using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalInCombatState : NormalState
{
    public NormalInCombatState(IStateContext stateContext) : base(stateContext)
    {
        _currentAnimID = Animator.StringToHash("isCombat");
        _currentLayerNameAnim = "Combat with meele weapon";
    }
    public override void CalculateTargerSpeed()
    {
        bool isSprint = PlayerAssetsInputs.instance.IsSprint();
        _targetSpeed = isSprint ? PlayerController.Instance.SprintSpeed * 1.126f : PlayerController.Instance.MoveSpeed * 1.5f;
        if (PlayerAssetsInputs.instance.GetMoveInput() == Vector2.zero) _targetSpeed = 0.0f;
    }
    public override void UpdateState(Animator _playerAnimator)
    {
        if (!PlayerAssetsInputs.instance.IsCombat())
        {
            int _layerAnimID = _playerAnimator.GetLayerIndex(CurrentLayerNameAnim);
            _playerAnimator.SetLayerWeight(_layerAnimID, 0);
            _playerAnimator.SetBool(_currentAnimID, false);

            _stateContext.ChangeState(new NormalState(_stateContext));
        }
        else if (PlayerAssetsInputs.instance.IsAiming() && !PlayerAssetsInputs.instance.IsSprint())
        {
            _stateContext.ChangeState(new CombatState(_stateContext));
            int _layerAnimID = _playerAnimator.GetLayerIndex(CurrentLayerNameAnim);

            _playerAnimator.SetLayerWeight(_layerAnimID, 1);
            _playerAnimator.SetBool(_currentAnimID, true);
        }else if (PlayerAssetsInputs.instance.IsAttack())
        {
            _stateContext.ChangeState(new AttackingState(_stateContext));

            _playerAnimator.SetBool(_currentAnimID, true);
        }
    }
}
