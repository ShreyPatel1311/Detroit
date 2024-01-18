public abstract class AUState
{
    public string name;
    public abstract void EnterState(ArmyUnitAI auAI);
    public abstract void UpdateState(ArmyUnitAI auAI);
}
