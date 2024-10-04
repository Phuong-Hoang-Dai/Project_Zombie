using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerState
{

    public DeadState(PlayerController.PlayerState stateKey) : base(stateKey)
    {
        _moveSpeed =0;
        _sprintSpeed = _moveSpeed;

        AnimID = Animator.StringToHash("isDead");
    }

    public override void EnterState()
    {
        base.EnterState();

        PlayerController.Instance.PlayerAnim.SetBool(AnimID, true);
        PlayerController.Instance.Dead();

        NextState = PlayerController.PlayerState.Dead;
    }
    public override void UpdateState()
    {
        return;
    }
}
