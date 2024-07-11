using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] float damage = 20f;
    [SerializeField] float shootForce = 15f;
    Rigidbody rb;
    BoxCollider boxCollider;
    NetworkObject networkObject;

    private bool hasHit = false; //helping bool because OnTrigger was called 2 times throwing error

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        networkObject = GetComponent<NetworkObject>();

        rb.AddForce(transform.forward.normalized * shootForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasHit && IsOwner)
        {
            hasHit = true;

            if (other.CompareTag("enemy"))
            {
                other.GetComponent<EnemyHealth>().GetDamage(damage);
            }

            boxCollider.isTrigger = false;

            // we need this to be called once (from host)
            if (IsHost)
            {
                DestroyObjectServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyObjectServerRpc()
    {
         StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(3f);

        networkObject.Despawn(true);
    }
}
