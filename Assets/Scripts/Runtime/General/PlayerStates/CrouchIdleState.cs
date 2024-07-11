using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchIdleState : PlayerState
{
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private Vector3 defaultColliderCenter = new Vector3(0, .9f, 0);
    [SerializeField] private float defaultCrouchHeight = 1.9f;

    public override void Enter(PlayerStateManager playerStateManager)
    {
        playerStateManager.ChangeLocomotion("CrouchSpeed", 0);
    }

    public override void Do(PlayerStateManager playerStateManager)
    {
        if (playerStateManager.moveInput != Vector2.zero)
        {
            playerStateManager.SwitchState(playerStateManager.crouchWalkState);
        }

        playerStateManager.playerController.Move(new Vector3(0, 0, 0));

        if (!playerStateManager.isCrouching)
        {
            playerStateManager.playerAnimator.SetBool("isCrouching", false);
            StartCoroutine(UnCrouch(playerStateManager));

            playerStateManager.SwitchState(playerStateManager.idleState);
        }
    }

    public override void FixedDo(PlayerStateManager playerStateManager)
    {
       
    }

    public override void Exit(PlayerStateManager playerStateManager)
    {

    }

    private IEnumerator UnCrouch(PlayerStateManager playerStateManager)
    {
        float elapsedTime = 0f;

        Vector3 currentColliderCenter = playerStateManager.playerController.center;
        float currentColliderHeight = playerStateManager.playerController.height;

        while (elapsedTime < crouchSpeed)
        {

            playerStateManager.playerController.center = Vector3.Lerp(currentColliderCenter, defaultColliderCenter, (elapsedTime / crouchSpeed));
            playerStateManager.playerController.height = Mathf.Lerp(currentColliderHeight, defaultCrouchHeight, (elapsedTime / crouchSpeed));

            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        playerStateManager.playerController.center = defaultColliderCenter;
        playerStateManager.playerController.height = defaultCrouchHeight;
        yield return null;

    }
}
