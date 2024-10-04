using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyState : BaseState<Zombie.ZombieState>
{
    protected Zombie _stateContext;
    protected float _rangeAtk;
    protected float _speed;
    protected int _currentAnimID;
    protected Vector3 moveTargetPosition;
    protected Vector3 lookTargetPosition;
    protected static float delayHit = 0f;
    protected readonly float delayHitTime = 3f;
    protected string _nameHitBox;

    protected int _speedAnimID = Animator.StringToHash("speed");

    public EnemyState(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey)
    {
        _stateContext = stateContext;
    }

    protected Vector3 CalculateUnitVecto() 
        => (_stateContext.transform.position - PlayerController.Instance.transform.position).normalized;

    protected void CalculateMoveTargetPositionByPlayer(float offset = 0, Vector3 rotation = new Vector3())
        => moveTargetPosition = PlayerController.Instance.transform.position 
            + Quaternion.Euler(rotation) * (offset * CalculateUnitVecto());
    protected void CalculateMoveTargetPositionByItself(float offset = 0, Vector3 rotation = new Vector3())
        => moveTargetPosition = _stateContext.transform.position
            + Quaternion.Euler(rotation) * (offset * CalculateUnitVecto());

    protected void CalculateLookTargetPosition()
        => lookTargetPosition = PlayerController.Instance.transform.position;

    protected void MoveToTarget()
    {
        if(moveTargetPosition != null)
        {
            Vector3 moveDirection = (moveTargetPosition - _stateContext.transform.position).normalized;

            _stateContext.ZombieController.Move(_speed * Time.deltaTime * moveDirection);
        }
    }

    protected void LookTarget() => _stateContext.transform.LookAt(lookTargetPosition);


    protected bool IsTargetInRangeAttack()
    {
        if(moveTargetPosition != null)
        {
            return Vector3.Distance(moveTargetPosition, 
                _stateContext.gameObject.transform.position) < _rangeAtk;
        }
        return false;
    }

    protected bool IsTargetInRangeAttack(float rangeAtk)
    {
        if (moveTargetPosition != null)
        {
            return Vector3.Distance(moveTargetPosition,
                _stateContext.gameObject.transform.position) < rangeAtk;
        }
        return false;
    }

    protected bool IsTargetInRange()
    {
        if (moveTargetPosition != null)
        {
            return Vector3.Distance(moveTargetPosition,
                _stateContext.gameObject.transform.position) < 50;
        }
        return false;
    }

    protected void StartAnim() => _stateContext.ZombieAnim.SetBool(_currentAnimID, true);
    protected void StopAnim() => _stateContext.ZombieAnim.SetBool(_currentAnimID, false);
    protected void TriggerAnim() => _stateContext.ZombieAnim.SetTrigger(_currentAnimID);

    public override void EnterState()
    {
        _stateContext.ZombieAnim.SetFloat(_speedAnimID, _speed);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if(delayHit > 0) delayHit -= Time.deltaTime;

        if (_stateContext.EnemyStat.CurrentHp <= 0) 
            NextState = Zombie.ZombieState.Dead;
    }
    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
    }
}

