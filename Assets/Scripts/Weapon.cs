using UnityEngine;

public abstract class Weapon : MonoBehaviour, IAttackable
{
    protected bool isAttacking = false;
    protected float _damageDealt;
    protected int _IDAttackAnimation;
    protected float _critRate;
    protected float _critDmg;
    protected float _damageMultiplier;
    public AudioClip attackAudioClip;

    public float DamageDealt => _damageDealt;
    public int IDAttackAnim => _IDAttackAnimation;
    public float CritRate => _critRate;
    public float CritDmg => _critDmg;
    public float DamageMultiplier => _damageMultiplier;

    public virtual void Attack(IDamageable enemy)
    {
        int isCrit = 0;
        int rand = Random.Range(0, 1000);
        if (rand <= 1000 * CritRate) isCrit = 1;

        float outgoingDamage = (_damageDealt + PlayerStats.instance.DamageDealt) * (1 - CritDmg * isCrit);

        enemy.TakeDamage(outgoingDamage);
    }
    public void SetDamageDealt(float newDMG) => _damageDealt = newDMG;
    public void SetCritRate(float newCritRate) => _critRate = newCritRate;
    public void SetCritDmg(float newCritDmg) => _critDmg = newCritDmg;
    public void SetDamageMultiplier(float newDmgDealt) => _damageMultiplier = newDmgDealt;
    public void StartAttackAnimation(Animator animController) => animController.SetBool(_IDAttackAnimation, true);
    public void StopAttackAnimation(Animator animController) => animController.SetBool(_IDAttackAnimation, false);
    public void StartAttack() => isAttacking = true;
    public void StopAttack() => isAttacking = false;

}
