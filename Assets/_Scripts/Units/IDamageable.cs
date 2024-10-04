public interface IDamageable
{
    public float MaxHp { get; set; }
    public float CurrentHp { get; set; }
    public float Def { get; set; }

    public void TakeDamage(float damage);
}
