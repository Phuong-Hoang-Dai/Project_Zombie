using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float hp = 10f;
    [SerializeField]
    private float def = 10f;

    public float Hp => hp; 
    public float Def => def;

    public void TakeDamage(float damage)
    {
        hp -= damage * (1 - (def / (def + 40)));
        if (hp <= 0) hp = 0;
    }
}
