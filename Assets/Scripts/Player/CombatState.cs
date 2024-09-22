using UnityEngine;
using UnityEngine.EventSystems;

public class CombatState : PlayerState
{
    protected float angle;

    public CombatState(IStateContext stateContext) : base(stateContext)
    {
        _currentAnimID = Animator.StringToHash("isAiming");

        _currentLayerNameAnim = "Aiming";
    }

    public override void CalculateTargerSpeed()
    {
        _targetSpeed = PlayerController.Instance.MoveSpeed * 0.75f;
        if (PlayerAssetsInputs.instance.GetMoveInput() == Vector2.zero) _targetSpeed = 0.0f;
    }

    public override void CalculateTargerAngle()
    {
        Vector3 inputDirection = Quaternion.Euler(0f, 45, 0f) * new Vector3(PlayerAssetsInputs.instance.GetMoveInput().x, 0.0f, PlayerAssetsInputs.instance.GetMoveInput().y).normalized;
        if (inputDirection != Vector3.zero)
            _inputRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

        Vector2 mouseDirection = PlayerAssetsInputs.instance.GetLookDirection();
        _mouseRotation = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;

        angle = Vector3.Angle(mouseDirection, new Vector2(inputDirection.x, inputDirection.z));
        Vector3 cross = Vector3.Cross(mouseDirection, new Vector2(inputDirection.x, inputDirection.z));
        if (cross.z > 0) angle = -angle;
    }
   
    public override void CalculateRotation(ref float rotation)
    {

        float _rotationSmoothTime = PlayerController.Instance.RotationSmoothTime;
        rotation = Mathf.SmoothDampAngle(PlayerController.Player.transform.eulerAngles.y,
            _mouseRotation, ref _rotationVelocity, _rotationSmoothTime);
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

        Vector3 animationSpeed = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

        _animationSpeed_X = Mathf.Lerp(_animationSpeed_X, animationSpeed.x * _targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationSpeed_X < 0.01f && _animationSpeed_X > -0.01f) _animationSpeed_X = 0f;

        _animationSpeed_Y = Mathf.Lerp(_animationSpeed_Y, animationSpeed.z * _targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationSpeed_Y < 0.01f && _animationSpeed_Y > -0.01f) _animationSpeed_Y = 0f;
    }

    public override void UpdateState(Animator _playerAnimator)
    {
        if (PlayerAssetsInputs.instance.IsSprint() || !PlayerAssetsInputs.instance.IsAiming() 
            || !PlayerAssetsInputs.instance.IsCombat())
        {
            int _layerAnimID = _playerAnimator.GetLayerIndex(CurrentLayerNameAnim);
            _playerAnimator.SetLayerWeight(_layerAnimID, 0);
            _playerAnimator.SetBool(_currentAnimID, false);
            
            _stateContext.ChangeState(new NormalInCombatState(_stateContext));
        }
        else if (PlayerAssetsInputs.instance.IsAttack())
        {
            _stateContext.ChangeState(new AttackingState(_stateContext));

            _playerAnimator.SetBool(_currentAnimID, true);
        }
    }

    public override void StartAttack() { return; }
    public override void StopAttack() { return; }
}
