using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey, stateContext)
    {
        _speed = 0;
    }
    public override void EnterState()
    {
        base.EnterState();
        NextState = Zombie.ZombieState.Idle;
    }
    public override void UpdateState()
    {
        CalculateMoveTargetPositionByPlayer();

        if (IsTargetInRange()) NextState = Zombie.ZombieState.Chase;
        else base.UpdateState();
    }
}
