using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    readonly float _stopAttackTime = 1.06f;
    readonly float _maxAttackTime = 2.17f;
    float _timer;

    public EnemyAttack(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey, stateContext)
    {
        _speed = 0;
        _currentAnimID = Animator.StringToHash("attack");
        _nameHitBox = "Hand_R";
        _rangeAtk = 1f;
    }

    public override void EnterState()
    {
        base.EnterState();
        NextState = Zombie.ZombieState.Attack;
        _timer = 0;
        StartAnim();
    }
    public override void UpdateState()
    {
        _timer += Time.deltaTime;

        CalculateMoveTargetPositionByPlayer();
        CalculateLookTargetPosition();
        LookTarget();

        if (IsTargetInRangeAttack() && _timer > _maxAttackTime)
        {
            _timer = 0;
        }
        else if (!IsTargetInRangeAttack() && _timer > _stopAttackTime)
        {
            StopAnim();
            NextState = Zombie.ZombieState.Chase;
        }

        base.UpdateState();
    }
    public override void ExitState()
    {
        base.ExitState();

        StopAnim();
    }

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if(collider.TryGetComponent(out IDamageable player))
        {
            AudioSource.PlayClipAtPoint(_stateContext.AttackSound,
                _stateContext.transform.position, 1);
            player.TakeDamage(_stateContext.EnemyStat.BaseAtk);
        }
    }
}
