using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{
    Animator animator;
    PlayerController playerController;

    [SerializeField] private float speedLerpTime = 0.1f; // Time to lerp the speed parameter
    [SerializeField] private float speedThreshold = 0.01f; // Threshold to snap to the target speed

    private float damage;

    public float animationBlendTime = 1f;

    private void Start()
    {
        if (!IsOwner) return;

        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    public void UpdateMovementAnimation()
    {
        if (!IsOwner) return;

        // Determine the target speed based on player movement
        float targetSpeed = 0f;
        if (playerController.speed == playerController.walkingSpeed)
        {
            targetSpeed = 0.5f;
        }
        else if (playerController.speed == playerController.runningSpeed)
        {
            targetSpeed = 1f;
        }

        // Lerp the StandingSpeed parameter
        animator.SetFloat("StandingSpeed", Mathf.Lerp(animator.GetFloat("StandingSpeed"), targetSpeed, speedLerpTime * Time.deltaTime));

        // Snap to the target speed if close enough
        if (Mathf.Abs(animator.GetFloat("StandingSpeed") - targetSpeed) < speedThreshold)
        {
            animator.SetFloat("StandingSpeed", targetSpeed);
        }

        // Update the IsMovingBackward parameter
        animator.SetBool("isMovingBackward", playerController.IsMovingBackward());
    }

    public void UpdateCrouchAnimation()
    {
        // Update the IsCrouching parameter
        animator.SetBool("isCrouching", playerController.isCrouching);
    }


    public void AttackTrigger()
    {
        animator.SetTrigger("attackTrigger");
    }

    public void OverrideAnimatorAttack(AnimatorOverrideController animatorOverrideController)
    {
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    public bool IsAttackEnded()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
    }

    public void PlayAttackAnim(int comboCounter)
    {
        animator.SetFloat("comboCounter", comboCounter);
    }

    public void SetWeaponAnimations(WeaponType weaponType)
    {
        animator.SetTrigger("changeWeapon");


        if(weaponType == WeaponType.Hands)
        {
            animator.SetInteger("weaponEquiped", 0);
        }
        else if(weaponType == WeaponType.Sword)
        {
            animator.SetInteger("weaponEquiped", 1);
        }
        else if (weaponType == WeaponType.Bow)
        {
            animator.SetInteger("weaponEquiped", 2);
        }
        else if(weaponType == WeaponType.Wand)
        {
            animator.SetInteger("weaponEquiped", 3);
        }
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("deadTrigger");
    }
}
