using System.Collections;
using UnityEngine;

public class JumpState : CommState
{
    private int count = 0;
    public override void EnterState(Commander1AI c1AI)
    {
        this.name = "jump";
        count = 0;
    }

    public override void UpdateState(Commander1AI c1AI)
    {
        if (c1AI.los.visibleEnemy.Contains(c1AI.Player))
        {
            c1AI.SwitchState(c1AI.teleport);
        }
        else
        {
            //animate commander.
            //and give teleport effect such that it teleports at the highest point of jump directly.
            if (c1AI.cc.isGrounded && count == 0)
            {
                Vector3 dir = c1AI.throwGre.LaunchDirection(c1AI, c1AI.gameObject);
                count = 1;
                c1AI.cc.Move(dir);
            }
            else if (count == 1 & c1AI.cc.isGrounded)
            {
                c1AI.GrenadeThrown = true;
                c1AI.SwitchState(c1AI.throwGre);
            }
        }
    }
}