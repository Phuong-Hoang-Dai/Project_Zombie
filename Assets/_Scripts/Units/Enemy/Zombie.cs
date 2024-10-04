using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : StateManager<Zombie.ZombieState>
{
    private List<BoxCollider> hitBox = new();
    public Animator ZombieAnim { get; protected set; }
    public CharacterController ZombieController { get; protected set; }
    public EnemyStat EnemyStat { get; protected set; }
    public AudioClip AttackSound;

    protected override void Awake()
    {
    }

    protected override void Start()
    {
        ZombieAnim = GetComponentInChildren<Animator>();
        ZombieController = GetComponent<CharacterController>();
        hitBox = GetComponentsInChildren<BoxCollider>().ToList();
        EnemyStat = GetComponent<EnemyStat>();

        states.Add(ZombieState.Idle, new EnemyIdle(ZombieState.Idle, this));
        states.Add(ZombieState.Chase, new EnemyChase(ZombieState.Chase, this));
        states.Add(ZombieState.Attack, new EnemyAttack(ZombieState.Attack, this));
        states.Add(ZombieState.Hit, new EnemyHit(ZombieState.Hit, this));
        states.Add(ZombieState.Dead, new EnemyDead(ZombieState.Dead, this));

        currentState = states[ZombieState.Idle];

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Dead()
    {
        Invoke(nameof(RemoveZombie), 3f);
    }

    public void RemoveZombie()
    {
        gameObject.SetActive(false);
    }

    public void StartAttack(string nameHitBox)
    {
        foreach(var hit in hitBox)
        {
            if (hit.name == nameHitBox)
                hit.enabled = true;
        }
    }

    public void StopAttack(string nameHitBox)
    {
        foreach (var hit in hitBox)
        {
            if (hit.name == nameHitBox)
                hit.enabled = false;
        }
    }

    public void IsHitByPlayer()
    {
        TransitionToState(ZombieState.Hit);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentState.OnTriggerEnter(other);
        }
    }

    public enum ZombieState
    {
        Idle,
        Chase,
        Attack,
        Hit,
        Dead
    }
}
