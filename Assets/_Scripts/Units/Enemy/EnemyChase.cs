using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyState
{
    public EnemyChase(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey, stateContext)
    {
        _speed = 1;
        _rangeAtk = 1f;
    }
    public override void EnterState()
    {
        base.EnterState();
        NextState = Zombie.ZombieState.Chase;
    }
    public override void UpdateState()
    {
        CalculateMoveTargetPositionByPlayer();
        CalculateLookTargetPosition();
        LookTarget();

        if (IsTargetInRangeAttack()) NextState = Zombie.ZombieState.Attack;
        else MoveToTarget();
        base.UpdateState();
    }
}
