using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandleAttack : NetworkBehaviour
{
    [SerializeField] InputReader inputReader;
  //  [SerializeField] float damage;

    [SerializeField] Transform playersBody;

    [SerializeField] LayerMask enemyLayer;
    public Vector2 mousePos;
    Vector3 target;

    private HandleEquipedItems handleEquipedItems;


    public List<CurrentlyEquipedWeapons> currentlyEquipedWeaponsList = new List<CurrentlyEquipedWeapons>();
    [System.Serializable]
    public class CurrentlyEquipedWeapons
    {
        public SO_Weapons soWeapon;
        public GameObject currentlyEquipedObject;
        public ulong clientId;
    }

    public void Start()
    {
        inputReader.LookEvent += InputReader_LookEvent;

        handleEquipedItems = GetComponentInParent<HandleEquipedItems>();
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    // melee animation event on attack
    public void Damage()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, enemyLayer))
        {
            if (hit.transform.GetComponent<EnemyHealth>())
            {
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();

                if (enemyHealth.canTakeDamage)
                {
                    enemyHealth.GetDamage(handleEquipedItems.currentlyEquipedItem.damage);
                   // enemyHealth.Hit(playersBody.gameObject);
                }
            }          
        }
    }

    // bow animation event on attack
    public void SpawnProjectile()
    {
        if (handleEquipedItems.currentlyEquipedItem.projectilePrefab != null)
        {
            SpawnProjectileServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId; //get client id who called the server rpc

        for (int i = 0; i < currentlyEquipedWeaponsList.Count; i++)
        {
            if (currentlyEquipedWeaponsList[i].clientId == clientId)
            {
                Transform projectileTransform = Instantiate(currentlyEquipedWeaponsList[i].soWeapon.projectilePrefab, currentlyEquipedWeaponsList[i].currentlyEquipedObject.transform.position, playersBody.rotation).transform;

                NetworkObject itemTransformNetworkObj = projectileTransform.GetComponent<NetworkObject>();

                itemTransformNetworkObj.Spawn(true);

                return;
            }
        }      
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target, 0.4f);
    }
}
