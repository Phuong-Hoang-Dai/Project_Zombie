using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : EnemyState
{
    readonly float _finishHit = 1.42f;
    float _timer;

    public EnemyHit(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey, stateContext)
    {
        _speed = 0;
        _currentAnimID = Animator.StringToHash("isHit");
    }
    public override void EnterState()
    {
        base.EnterState();

        if(delayHit > 0)
        {
            NextState = Zombie.ZombieState.Chase;
        }
        else
        {
            NextState = Zombie.ZombieState.Hit;
            _timer = 0;
            StartAnim();
        }
    }
    public override void UpdateState()
    {
        _timer += Time.deltaTime;

        if (_timer > _finishHit )
        {
            delayHit = delayHitTime;
            StopAnim();
            NextState = Zombie.ZombieState.Chase;
        }

        base.UpdateState();
    }
    public override void ExitState()
    {
        StopAnim();
    }
}
