using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerState : BaseState<PlayerController.PlayerState>
{
    public string LayerNameAnim { get; protected set; }
    public int LayerAnimID { get; protected set; }
    public int AnimID { get; protected set; }

    protected float _moveSpeed;
    protected float _sprintSpeed;
    protected float _speedOffset;
    protected float _rotationSmoothTime;
    protected float _speedChangeRate;

    protected Vector3 inputDirection;
    protected float _targetSpeed;
    protected float _inputRotation;
    protected float _mouseRotation;
    protected float _rotationVelocity;
    protected float _angle;

    protected static float _speed;
    protected static float _rotation;
    protected static float _animationSpeed_X;
    protected static float _animationSpeed_Y;

    protected readonly static int _animIDSpeed_X = Animator.StringToHash("Speed_X");
    protected readonly static int _animIDSpeed_Y = Animator.StringToHash("Speed_Y");
    protected readonly static int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    public PlayerState(PlayerController.PlayerState stateKey) : base(stateKey)
    {
        _moveSpeed = PlayerController.Instance.Stats.MoveSpeed;
        _sprintSpeed = PlayerController.Instance.Stats.SprintSpeed;
        _speedOffset = PlayerController.Instance.Stats.SpeedOffset;
        _rotationSmoothTime = PlayerController.Instance.Stats.RotationSmoothTime;
        _speedChangeRate = PlayerController.Instance.Stats.SpeedChangeRate;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        CalculateTargerSpeed();
        CalculateTargerAngle();
        
        HandleMovement();
        HandleRotation(_inputRotation);
        HandleAnimation();
    }

    protected void CalculateTargerSpeed()
    {
        bool isSprint = InputManager.Instance.IsSprint;

        _targetSpeed = isSprint ? _sprintSpeed : _moveSpeed;

        if (InputManager.Instance.MoveInput == Vector2.zero) _targetSpeed = 0.0f;
    }

    protected void CalculateTargerAngle()
    {
        inputDirection = new Vector3(
            InputManager.Instance.MoveInput.x, 0.0f, InputManager.Instance.MoveInput.y).normalized;

        if (inputDirection != Vector3.zero)
            _inputRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg ;
        else
            _inputRotation = PlayerController.Instance.transform.eulerAngles.y;
    }

    protected void CalculateLookAngle()
    {
        Vector2 mouseDirection = InputManager.Instance.GetLookDirection();

        _mouseRotation = Mathf.Atan2(mouseDirection.x, mouseDirection.y) * Mathf.Rad2Deg;

        _angle = Vector3.Angle(mouseDirection, new Vector2(inputDirection.x, inputDirection.z));
        Vector3 cross = Vector3.Cross(mouseDirection, new Vector2(inputDirection.x, inputDirection.z));

        if (cross.z > 0) _angle = -_angle;
    }

    protected void HandleRotation(float angleToRotate)
    {
        _rotation = Mathf.SmoothDampAngle(PlayerController.Instance.Player.transform.eulerAngles.y,
            angleToRotate, ref _rotationVelocity, _rotationSmoothTime);

        PlayerController.Instance.Player.transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
    }

    protected void HandleMovement()
    {
        if (_speed < _targetSpeed - _speedOffset || _speed > _targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_speed, _targetSpeed, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000) / 1000;
        }
        else _speed = _targetSpeed;

        Vector3 moveDirection = (Quaternion.Euler(0.0f, _inputRotation, 0.0f) * Vector3.forward).normalized;

        PlayerController.Instance.CharacterController.Move(_speed * Time.deltaTime * moveDirection);
    }

    protected void HandleAnimation()
    {
        Vector3 animationSpeed = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward;

        _animationSpeed_X = Mathf.Lerp(_animationSpeed_X, animationSpeed.x * _targetSpeed,
            Time.deltaTime * _speedChangeRate);
        if (_animationSpeed_X < 0.01f && _animationSpeed_X > -0.01f) _animationSpeed_X = 0f;

        _animationSpeed_Y = Mathf.Lerp(_animationSpeed_Y, animationSpeed.z * _targetSpeed,
            Time.deltaTime * _speedChangeRate);
        if (_animationSpeed_Y < 0.01f && _animationSpeed_Y > -0.01f) _animationSpeed_Y = 0f;

        PlayerController.Instance.PlayerAnim.SetFloat(_animIDSpeed_X, _animationSpeed_X);
        PlayerController.Instance.PlayerAnim.SetFloat(_animIDSpeed_Y, _animationSpeed_Y);
        PlayerController.Instance.PlayerAnim.SetFloat(_animIDMotionSpeed, 1f);
    }
}

