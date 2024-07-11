using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    [SerializeField] private float runSpeed = 10f;

    public override void Do(PlayerStateManager playerStateManager) {

        if (playerStateManager.moveInput != Vector2.zero) {
            playerStateManager.moveDir = new Vector3(playerStateManager.moveInput.x, 0, playerStateManager.moveInput.y) * Time.deltaTime;
        }
        playerStateManager.playerController.Move(playerStateManager.moveAmmount + playerStateManager.moveDir * runSpeed);

        if (playerStateManager.moveInput == Vector2.zero) {
            playerStateManager.SwitchState(playerStateManager.idleState);

        }

        if (playerStateManager.isRunning == false || playerStateManager.playerVitals.CurrentStamina == 0) {
            playerStateManager.SwitchState(playerStateManager.walkState);
        }
        if (!playerStateManager.playerSensors.isGrounded) {
            playerStateManager.SwitchState(playerStateManager.airborneState);
        }

    }

    public override void Enter(PlayerStateManager playerStateManager) {
        playerStateManager.ChangeLocomotion("StandingSpeed", 1);
        Debug.Log("RUN");

    }

    public override void Exit(PlayerStateManager playerStateManager) {
        
    }

    public override void FixedDo(PlayerStateManager playerStateManager) {


    }
}
