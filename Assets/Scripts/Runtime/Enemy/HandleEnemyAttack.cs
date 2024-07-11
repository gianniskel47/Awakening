using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandleEnemyAttack : NetworkBehaviour
{
    [SerializeField] SO_Weapons soWeaponEquiped;
    [SerializeField] Transform enemyWeapon;
    [SerializeField] Transform enemyBody;


    // animation event for melee enemies
    public void DamagePlayer()
    {
        enemyWeapon.GetComponent<EnemyMeleeWeapon>().DamagePlayer(soWeaponEquiped.damage);
    }


    // animation event for ranged enemies
    public void SpawnProjectile()
    {
        // we need this to happen once per animation (only on host)
        if (IsHost)
        {
            if (soWeaponEquiped.enemyProjectilePrefab != null)
            {
                SpawnProjectileServerRpc();
            }
            else
            {
                Debug.Log("NO PROJECTILE PREFAB ON SO_WEAPON");
            }
        }      
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnProjectileServerRpc()
    {
        Transform projectileTransform = Instantiate(soWeaponEquiped.enemyProjectilePrefab, enemyWeapon.position, enemyBody.rotation).transform;

        NetworkObject itemTransformNetworkObj = projectileTransform.GetComponent<NetworkObject>();

        itemTransformNetworkObj.Spawn(true);
    }
}
