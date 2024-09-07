using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour, IDamageable
{
    private Vector3 targetPosition;

    private float _damageDealt;
    private float attackRange = 1f;
    private float hp = 10f;

    private bool isWaking = true;
    private bool isDead = false;

    public float Speed;
    private float speed;

    private int _animIDSpeed;
    private int _animIDAttack;
    private int _animIDDead;
    private int _animIDHit;

    private BoxCollider hitBox;
    private Animator zombieAnim;
    private CharacterController zombie;

    public float DamageDealt { get => _damageDealt; }
    public float Hp { get => hp; }

    void Start()
    {
        zombieAnim = GetComponent<Animator>();
        zombie = GetComponent<CharacterController>();
        hitBox = GetComponent<BoxCollider>();

        AssignAnimtionIDs();
        SetTargetPosition(PlayerController.Player.transform.position);
    }

    private void AssignAnimtionIDs()
    {
        _animIDSpeed = Animator.StringToHash("speed");
        _animIDAttack = Animator.StringToHash("attack");
        _animIDDead = Animator.StringToHash("isDead");
        _animIDHit = Animator.StringToHash("isHit");
    }

    void Update()
    {
        if (isDead) return;
        SetTargetPosition(PlayerController.Player.transform.position);

        if (IsPLayerOutOfAttackRange())
        {
            isWaking = true;
        }
        else if(isWaking)
        {
            isWaking = false;
            StartAttackAnimation();
        }

        speed = isWaking ? Speed : 0;
        if(speed > 0.01)
        {
            MoveToTargetPosition();
        }
        HandleAnimation();
    }

    private bool IsPLayerOutOfAttackRange()
    {
        return Vector3.Distance(transform.position, targetPosition) > attackRange;
    }

    private void MoveToTargetPosition()
    {
        transform.LookAt(targetPosition);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        zombie.Move(moveDirection * speed * Time.deltaTime);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }

    private void HandleAnimation()
    {
        zombieAnim.SetFloat(_animIDSpeed, speed);
    }

    private void StartAttackAnimation()
    {
        zombieAnim.SetTrigger(_animIDAttack);
    }

    public void StartAttack()
    {
        hitBox.enabled = true;
    }

    public void StopAttack()
    {
        hitBox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Zombie is hit");
        hp -= damage;
        if(hp <= 0)
        {
            isDead = true;
            hp = 0;
            zombie.enabled = false;
            zombieAnim.SetTrigger(_animIDDead);
        }
        else
        {
            zombieAnim.SetTrigger(_animIDHit);
        }
    }
}
