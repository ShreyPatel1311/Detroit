using UnityEngine;

public class OpenFireState : AUState
{
    private int counter = 0;
    public override void EnterState(ArmyUnitAI auAI)
    {
        this.name = "Open Fire";
        auAI.nav.speed = 1f;
        auAI.anim.SetBool("Sprint", false);
    }

    public override void UpdateState(ArmyUnitAI auAI)
    {
        if (Vector3.Distance(auAI.transform.position, auAI.player.transform.position) <= auAI.ChaseRadius)
        {
            if(counter == 0)
            {
                float x = Random.Range(1, 3);
                float y = Random.Range(1, 3);
                float z = Random.Range(1, 3);

                Vector3 destPoint = auAI.player.transform.position + new Vector3(x, y, z);

                auAI.nav.SetDestination(destPoint);
                counter = 1;
            }
            else
            {

            }
        }
        else
        {
            auAI.transform.LookAt(auAI.player.transform.position);
        }
    }
}
