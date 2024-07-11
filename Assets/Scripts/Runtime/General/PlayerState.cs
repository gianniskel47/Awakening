using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public virtual void Enter(PlayerStateManager playerStateManager) { }
    public virtual void Do(PlayerStateManager playerStateManager) { }
    public virtual void FixedDo(PlayerStateManager playerStateManager) { }
    public virtual void Exit(PlayerStateManager playerStateManager) { }
}
