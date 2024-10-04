using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : PlayerState
{
    private readonly float minAttackTime = .55f;
    private readonly float maxAttackTime = 1.07f;

    float timer = 0;

    public AttackingState(PlayerController.PlayerState stateKey) : base(stateKey)
    {
        _moveSpeed = 0;
        _sprintSpeed = 0;

        AnimID = Animator.StringToHash("isAttacking");
    }

    public override void EnterState()
    {
        base.EnterState();

        NextState = PlayerController.PlayerState.Attacking;

        PlayerController.Instance.PlayerAnim.SetBool(AnimID, true);
        PlayerController.Instance.StartAttack();
    }
    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (CanCancel())
        {
            PlayerController.Instance.PlayerAnim.SetBool(AnimID, false);
            PlayerController.Instance.StopAttack();
        }

        if (CanCancel() && InputManager.Instance.IsAttack)
        {
            timer = 0;
            PlayerController.Instance.PlayerAnim.SetBool(AnimID, true);
            PlayerController.Instance.StartAttack();
        }
        else if (CanChangeState()) 
        {
            timer = 0;
            NextState = PlayerController.PlayerState.Aiming;
        }
        else
        {
            CalculateTargerSpeed();
            CalculateTargerAngle();
            CalculateLookAngle();

            HandleMovement();
            HandleRotation(_mouseRotation);
            HandleAnimation();
        }
    }
    private bool CanCancel() => timer > minAttackTime;
    private bool CanChangeState() => timer > maxAttackTime;
}
