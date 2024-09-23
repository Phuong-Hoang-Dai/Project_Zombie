public interface IAttackable
{
    public float DamageDealt { get;}
    public void Attack(IDamageable enemy);
}
