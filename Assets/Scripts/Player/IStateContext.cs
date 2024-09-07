public interface IStateContext 
{
    public PlayerState State { get; }
    public void ChangeState(PlayerState newState);
    public void StartAttack();
    public void StopAttack();
}
