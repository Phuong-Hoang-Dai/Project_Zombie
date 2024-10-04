public interface IAttackable
{
    public float BaseAtk { get;}
    public void Attack(IDamageable enemy);
}
