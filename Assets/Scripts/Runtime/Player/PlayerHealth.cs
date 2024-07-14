using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    BaseStats baseStats;

    [SerializeField] private float maxHealth = 1000;

    [SerializeField] float currentHealth;

    [SerializeField] InputReader inputReader;

    AnimationController animationController;

    private bool isDead = false;

    private void Start()
    {
        if (!IsOwner) return;

        animationController = GetComponentInChildren<AnimationController>();
        baseStats = GetComponent<BaseStats>();
        maxHealth = baseStats.GetStat(Stat.Health);
        currentHealth = maxHealth;
    }

    public void OnDamage(float damage)
    {
        if (!IsOwner || isDead) return;

        currentHealth -= damage;

        if(currentHealth < 0)
        {
            currentHealth = 0;
        }

        if(currentHealth <= 0)
        {
            inputReader.DisableOnFootInputs();
            animationController.PlayDeathAnimation();
            isDead = true;
            UpdateTagServerRpc();
        }
    }

/*    public void UpdateMaxHealth(Component component, object sender)
    {
        Progression progression = (Progression)sender;

        maxHealth = progression.GetStat(Stat.Health);
    }*/

    [ServerRpc(RequireOwnership = false)]
    private void UpdateTagServerRpc()
    {
        UpdateTagClientRpc();
    }

    [ClientRpc]
    private void UpdateTagClientRpc()
    {
        this.tag = "Untagged";
    }
}
