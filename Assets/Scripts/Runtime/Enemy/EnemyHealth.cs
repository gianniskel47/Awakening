using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : NetworkBehaviour
{
    public NetworkVariable<float> health = new NetworkVariable<float>(100);
    public bool canTakeDamage = false;
    BlazeAI blazeAI;

    private void Start()
    {
        blazeAI = GetComponent<BlazeAI>();
    }

    public void Hit(GameObject gameObj)
    {
        blazeAI.Hit(gameObj);
    }

    public void GetDamage(float damage)
    {

        GetDamageServerRpc(damage);

    }

    [ServerRpc(RequireOwnership = false)]
    private void GetDamageServerRpc(float damage)
    {
        health.Value -= damage;

        if (health.Value <= 0)
        {
            UpdateOnDeathClientRpc();
        }
    }

    [ClientRpc]
    private void UpdateOnDeathClientRpc()
    {
        blazeAI.Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            canTakeDamage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            canTakeDamage = false;
        }
    }
}
