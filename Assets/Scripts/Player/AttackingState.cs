using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : CombatState
{
    float minAttackTime = .55f;
    float maxAttackTime = 1.07f;
    float timer = 0;
    Vector2 lookDirection;
    public AttackingState(IStateContext stateContext) : base(stateContext)
    {
        _currentAnimID = Animator.StringToHash("isAttacking");
        lookDirection = PlayerAssetsInputs.Instance.GetLookDirection();
        PlayerController.Instance.StartAttack();
    }

    public override void UpdateState(Animator _playerAnimator)
    {
        timer += Time.deltaTime;
        if (CanCancel())
        {
            _playerAnimator.SetBool(_currentAnimID, false);
            PlayerController.Instance.StopAttack();
        }
        if (CanChangeState() || !PlayerAssetsInputs.Instance.IsCombat())
        {
            _stateContext.ChangeState(new CombatState(_stateContext));
        }
    }
    public override void CalculateTargerAngle()
    {
        Vector3 inputDirection = Quaternion.Euler(0f, 45, 0f) * new Vector3(PlayerAssetsInputs.Instance.GetMoveInput().x, 0.0f, PlayerAssetsInputs.Instance.GetMoveInput().y).normalized;
        if (inputDirection != Vector3.zero)
            _inputRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        _mouseRotation = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;

        angle = Vector3.Angle(lookDirection, new Vector2(inputDirection.x, inputDirection.z));
        Vector3 cross = Vector3.Cross(lookDirection, new Vector2(inputDirection.x, inputDirection.z));
        if (cross.z > 0) angle = -angle;
    }
    public override void CalculateTargerSpeed()
    {
        _targetSpeed = 0;
    }
    private bool CanCancel() => timer > minAttackTime;
    private bool CanChangeState() => timer > maxAttackTime;
}
