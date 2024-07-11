using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float maxSlopeAngle;
    public override void Do(PlayerStateManager playerStateManager) {
        if (playerStateManager.moveInput != Vector2.zero) {
            playerStateManager.moveDir = new Vector3(playerStateManager.moveInput.x, 0, playerStateManager.moveInput.y) * Time.deltaTime;
        }
        playerStateManager.playerController.Move(playerStateManager.moveAmmount + playerStateManager.moveDir * walkSpeed);

        if (!playerStateManager.playerSensors.isGrounded) {
            playerStateManager.SwitchState(playerStateManager.airborneState);
        }
        if (playerStateManager.moveInput == Vector2.zero) { 
            playerStateManager.SwitchState(playerStateManager.idleState);
        }

        if (playerStateManager.isRunning) {
            playerStateManager.SwitchState(playerStateManager.runState);
        }

        if (playerStateManager.isCrouching) {
            playerStateManager.SwitchState(playerStateManager.clouchState);
        }


    }

    public override void Enter(PlayerStateManager playerStateManager) {

        playerStateManager.ChangeLocomotion("StandingSpeed", 0.5f);
    }

    public override void Exit(PlayerStateManager playerStateManager) {
        
    }

    public override void FixedDo(PlayerStateManager playerStateManager) {

    }
}
