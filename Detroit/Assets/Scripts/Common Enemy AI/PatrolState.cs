using UnityEngine;

public class PatrolState : AUState
{
    private bool walkPointSet;
    public override void EnterState(ArmyUnitAI auAI)
    {
        auAI.nav.speed = 1f;
        walkPointSet = false;
        this.name = "patrol";
        auAI.anim.SetLayerWeight(1, 0);
    }

    public override void UpdateState(ArmyUnitAI auAI)
    {
        if (!auAI.los.visibleEnemy.Contains(auAI.player))
        {
            if (!walkPointSet)
            {
                auAI.anim.SetBool("move", true);
                auAI.nav.SetDestination(GetWayPoint(auAI));
            }
            if (auAI.nav.remainingDistance <= auAI.nav.stoppingDistance)
            {
                walkPointSet = false;
            }
        }
        else
        {
            AlertOtherUnits(auAI);
            auAI.SwitchState(auAI.Cover);
        }
    }

    private Vector3 GetWayPoint(ArmyUnitAI auAI)
    {
        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        Vector3 destPoint = new Vector3(auAI.spawnLocations.position.x + x, auAI.spawnLocations.position.y, auAI.spawnLocations.position.z + z);
        walkPointSet = true;

        return destPoint;
    }

    private void AlertOtherUnits(ArmyUnitAI auAI)
    {
        ArmyUnitAI currUnit;
        foreach(GameObject soldier in auAI.cm.allSoldiers)
        {
            currUnit = soldier.GetComponent<ArmyUnitAI>();
            if(Vector3.Distance(soldier.transform.position, auAI.gameObject.transform.position) < auAI.AlertRadius)
            {
                currUnit.SwitchState(currUnit.Cover);
            }
        }
    }
}