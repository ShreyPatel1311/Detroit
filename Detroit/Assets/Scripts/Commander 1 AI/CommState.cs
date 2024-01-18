public abstract class CommState
{
    public string name;
    public abstract void EnterState(Commander1AI c1AI);
    public abstract void UpdateState(Commander1AI c1AI);
}