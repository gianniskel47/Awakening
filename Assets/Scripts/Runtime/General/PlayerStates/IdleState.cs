using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void Do(PlayerStateManager playerStateManager) {
        playerStateManager.playerController.Move(Vector3.zero);

        if (playerStateManager.moveInput != Vector2.zero) {
            playerStateManager.SwitchState(playerStateManager.walkState);
        }

        if (playerStateManager.isCrouching) {
            playerStateManager.SwitchState(playerStateManager.clouchState);
        }
    }

    public override void Enter(PlayerStateManager playerStateManager) {
        playerStateManager.ChangeLocomotion("StandingSpeed", 0);

    }

    public override void Exit(PlayerStateManager playerStateManager) {
        
    }

    public override void FixedDo(PlayerStateManager playerStateManager) {

    }
}
