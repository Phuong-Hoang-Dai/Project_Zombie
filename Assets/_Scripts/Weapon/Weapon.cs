using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsAttacking {get; protected set;}

    public int IDAttackAnimation { get; protected set; }
    [field: SerializeField]
    public AudioClip AttackAudioClip { get; protected set; }

    public void StartAttack() => IsAttacking = true;
    public void StopAttack() => IsAttacking = false;
}
