using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    BaseStats baseStats;

    [SerializeField] private float health = 1000;
    [SerializeField] InputReader inputReader;

    AnimationController animationController;

    private bool isDead = false;

    private void Start()
    {
        if (!IsOwner) return;

        animationController = GetComponentInChildren<AnimationController>();
        baseStats = GetComponent<BaseStats>();
        health = baseStats.GetStat(Stat.Health);
    }

    public void OnDamage(float damage)
    {
        if (!IsOwner || isDead) return;

        health -= damage;

        if(health <= 0)
        {
            inputReader.DisableOnFootInputs();
            animationController.PlayDeathAnimation();
            isDead = true;
            UpdateTagServerRpc();
        }
    }

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
