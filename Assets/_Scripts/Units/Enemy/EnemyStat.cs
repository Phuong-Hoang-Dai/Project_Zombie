using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IDamageable, IAttackable
{
    protected Zombie zombie;

    [field: SerializeField]
    public float MaxHp {  get; set; }
    public float CurrentHp { get; set; }

    [field: SerializeField]
    public float Def { get; set; }

    [field: SerializeField]
    public float BaseAtk { get; set; }

    private void Start()
    {
        zombie = GetComponent<Zombie>();

        CurrentHp = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage * (1 - (Def / (Def + 40)));
        if (CurrentHp <= 0) CurrentHp = 0;

        zombie.IsHitByPlayer();
    }

    public void Attack(IDamageable enemy)
    {
        
    }
}
