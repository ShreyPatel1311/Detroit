using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : AUState
{
    public override void EnterState(ArmyUnitAI auAI)
    {
        this.name = "chase";
    }

    public override void UpdateState(ArmyUnitAI auAI)
    {
        
    }
}
