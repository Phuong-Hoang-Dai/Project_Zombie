using UnityEngine;

public abstract class PlayerState
{
    protected IStateContext _stateContext;
    protected float _rotationVelocity;
    protected float _inputRotation;
    protected float _mouseRotation;
    protected float _targetSpeed;
    protected static string _currentLayerNameAnim;
    protected static int _currentAnimID;
    public static string CurrentLayerNameAnim => _currentLayerNameAnim;
    public static int CurrentAnimID => _currentAnimID;

    public PlayerState(IStateContext stateContext) => this._stateContext = stateContext;

    public abstract void CalculateTargerSpeed();
    public abstract void CalculateTargerAngle();
    public abstract void CalculateRotation(ref float rotation);
    public abstract void CalculateMovement(ref float _speed, out Vector3 moveDirection);
    public abstract void CalculateAnimationBlend(ref float _animationSpeed_X, ref float _animationSpeed_Y);
    public abstract void StartAttack();   
    public abstract void StopAttack();
    public abstract void UpdateState(Animator _playerAnimator);
}
