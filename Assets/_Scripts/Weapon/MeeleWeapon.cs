using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeapon : Weapon
{
    protected List<IDamageable> _damagedEnemies = new();

    protected void Start()
    {
        IDAttackAnimation = Animator.StringToHash("MeeleAttack");
    }

    protected void Update()
    {
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            if (other.TryGetComponent(out IDamageable enemy))
            {
                if (!_damagedEnemies.Contains(enemy))
                {
                    _damagedEnemies.Remove(enemy);
                    AudioSource.PlayClipAtPoint(AttackAudioClip, transform.position);
                    PlayerController.Instance.Attack(enemy);
                }
            }
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable enemy))
        {
            _damagedEnemies.Remove(enemy);
        }
    }

}
