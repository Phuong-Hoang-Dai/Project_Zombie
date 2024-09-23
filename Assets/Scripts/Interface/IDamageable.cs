public interface IDamageable
{
    public float Hp { get; }
    public float Def { get; }
    public void TakeDamage(float damage);
}
