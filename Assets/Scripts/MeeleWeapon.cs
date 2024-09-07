using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : Weapon
{
    private BoxCollider hitBox;
    
    private void Start()
    {
        _damageDealt = 4f;

        hitBox = GetComponent<BoxCollider>();
        _IDAttackAnimation = Animator.StringToHash("MeeleAttack");
    }
    public override void Attack()
    {
        if(isAttacking)
        {
            hitBox.enabled = true;
        }
        else
        {
            hitBox.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isAttacking)
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if(enemy != null)
            {
                AudioSource.PlayClipAtPoint(attackAudioClip, transform.position); 
                enemy.TakeDamage(_damageDealt);
            }
        }
    }
}
