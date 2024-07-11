using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public bool canDamage = false;
    PlayerHealth playerHealth;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDamage = true;
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDamage = false;
            playerHealth = null;
        }
    }

    public void DamagePlayer(float damage)
    {
        if (canDamage && playerHealth != null)
        {
            playerHealth.OnDamage(damage);   
        }
    }
}
