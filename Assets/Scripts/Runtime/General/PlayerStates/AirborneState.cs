using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AirborneState : PlayerState
{
    [SerializeField] private float gravity = -9.18f;
    private Vector3 velocity = Vector3.zero;
    public override void Enter(PlayerStateManager playerStateManager) {
        //play animation pesimo
    }

    public override void Do(PlayerStateManager playerStateManager) {

        velocity.y += gravity * 2  * Time.deltaTime;
        playerStateManager.playerController.Move(velocity * Time.deltaTime);



        if (playerStateManager.playerSensors.isGrounded) {
            playerStateManager.SwitchState(playerStateManager.idleState);
        }
    }
}
