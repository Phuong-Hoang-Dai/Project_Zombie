using Unity.VisualScripting;
using UnityEngine;

public class NormalState : PlayerState
{
    public NormalState(IStateContext stateContext) : base(stateContext) => _currentLayerNameAnim = "Normal";

    public override void CalculateTargerSpeed()
    {
        bool isSprint = PlayerAssetsInputs.Instance.IsSprint();
        _targetSpeed = isSprint ? PlayerController.Instance.SprintSpeed : PlayerController.Instance.MoveSpeed;
        if (PlayerAssetsInputs.Instance.GetMoveInput() == Vector2.zero) _targetSpeed = 0.0f;
    }

    public override void CalculateTargerAngle()
    {
        Vector3 inputDirection = new Vector3(PlayerAssetsInputs.Instance.GetMoveInput().x, 0.0f, PlayerAssetsInputs.Instance.GetMoveInput().y).normalized;
        if (inputDirection != Vector3.zero)
            _inputRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + 45;
        else
            _inputRotation = PlayerController.Player.transform.eulerAngles.y;
    }

    public override void CalculateRotation(ref float rotation)
    {
        float _rotationSmoothTime = PlayerController.Instance.RotationSmoothTime;

        rotation = Mathf.SmoothDampAngle(PlayerController.Player.transform.eulerAngles.y,
        _inputRotation, ref _rotationVelocity, _rotationSmoothTime);
    }

    public override void CalculateMovement(ref float _speed, out Vector3 moveDirection)
    {
        float _speedOffset = PlayerController.Instance.SpeedOffset;
        float _speedChangeRate = PlayerController.Instance.SpeedChangeRate;

        if (_speed < _targetSpeed - _speedOffset || _speed > _targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_speed, _targetSpeed, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000) / 1000;
        }
        else _speed = _targetSpeed;

        moveDirection = (Quaternion.Euler(0.0f, _inputRotation, 0.0f) * Vector3.forward).normalized;
    }

    public override void CalculateAnimationBlend(ref float _animationSpeed_X, ref float _animationSpeed_Y)
    {
        float _speedChangeRate = PlayerController.Instance.SpeedChangeRate;

        _animationSpeed_Y = Mathf.Lerp(_animationSpeed_Y, _targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationSpeed_Y < 0.01f) _animationSpeed_Y = 0f;

        _animationSpeed_X = 0.0f;
    }

    public override void UpdateState(Animator _playerAnimator)
    {
        if (PlayerAssetsInputs.Instance.IsCombat())
        {
            _stateContext.ChangeState(new NormalInCombatState(_stateContext));

            int _layerAnimID = _playerAnimator.GetLayerIndex(CurrentLayerNameAnim);

            _playerAnimator.SetLayerWeight(_layerAnimID, 1);
            _playerAnimator.SetBool(_currentAnimID, true);
        }
    }

    public override void StartAttack() { return; }
    public override void StopAttack() { return; }

    
}
