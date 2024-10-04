using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyState
{
    protected float t = 0.5f;
    protected float time;

    public EnemyDead(Zombie.ZombieState stateKey, Zombie stateContext) : base(stateKey, stateContext)
    {
        _speed = 0;
        _currentAnimID = Animator.StringToHash("isDead");
    }
    public override void EnterState()
    {
        base.EnterState();
        NextState = Zombie.ZombieState.Dead;
        time = 0;
        TriggerAnim();
        _stateContext.ZombieController.enabled = false;
        _stateContext.Dead();

        InventoryManager.Instance.UpdateCoin(2);
        LevelManager.Instance.UpdateQuantityKill();
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;
        if( time > 1f) TranslateBody();

        return;
    }

    private void TranslateBody()
    {
        if (_stateContext.transform.position.y > -0.8f)
            _stateContext.transform.Translate(t * Time.deltaTime * Vector3.down);
    }
}
