using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : PlayerState
{
    private Vector3 moveAmount = Vector3.zero;

    [SerializeField] private float crouchMoveSpeed = 2.5f;
    [SerializeField] private Vector3 crouchColliderCenter = new Vector3(0,.6f,0);
    [SerializeField] private float crouchHeight = 1.3f;
    [SerializeField] private float crouchSpeed = 1f;

    [SerializeField] private Vector3 defaultColliderCenter = new Vector3 (0,.9f,0);
    [SerializeField] private float defaultCrouchHeight = 1.9f;
    //crouch default values center 0.9 ,    height 1.9
    //center .6,height 1.3 
    public override void Enter(PlayerStateManager playerStateManager) {
        playerStateManager.playerAnimator.SetBool("isCrouching", true);
        StartCoroutine(Crouch(playerStateManager));
    }

    public override void FixedDo(PlayerStateManager playerStateManager) {

        
    }

    public override void Do(PlayerStateManager playerStateManager) {
        if (playerStateManager.moveInput != Vector2.zero) {
            //walk crouch
            playerStateManager.SwitchState(playerStateManager.crouchWalkState);
        }
        else 
        {
            //idle crouch         
            playerStateManager.SwitchState(playerStateManager.crouchIdleState);
        }

        if (!playerStateManager.isCrouching) {
            playerStateManager.playerAnimator.SetBool("isCrouching", false);

            StartCoroutine(UnCrouch(playerStateManager));

            playerStateManager.SwitchState(playerStateManager.idleState);
        }
    }

    public override void Exit(PlayerStateManager playerStateManager) {
       // playerStateManager.playerAnimator.SetBool("isCrouching", false);

       // StartCoroutine(UnCrouch(playerStateManager));
    }

    private IEnumerator Crouch(PlayerStateManager playerStateManager) {
        float elapsedTime = 0f;

        Vector3 currentColliderCenter = playerStateManager.playerController.center;
        float currentColliderHeight = playerStateManager.playerController.height;

        while (elapsedTime < crouchSpeed) {


            playerStateManager.playerController.center = Vector3.Lerp(currentColliderCenter, crouchColliderCenter, (elapsedTime / crouchSpeed));
            playerStateManager.playerController.height = Mathf.Lerp(currentColliderHeight, crouchHeight, (elapsedTime / crouchSpeed));

            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        playerStateManager.playerController.center = crouchColliderCenter;
        playerStateManager.playerController.height = crouchHeight;
        yield return null;
    }


    private IEnumerator UnCrouch(PlayerStateManager playerStateManager) {
        float elapsedTime = 0f;

        Vector3 currentColliderCenter = playerStateManager.playerController.center;
        float currentColliderHeight = playerStateManager.playerController.height;

        while (elapsedTime < crouchSpeed) {

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
