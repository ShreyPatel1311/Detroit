using System.Collections.Generic;
using UnityEngine;

public class TeleportState : CommState
{
    private List<Transform> visiblePoints = new();

    public override void EnterState(Commander1AI c1AI)
    {
        this.name = "Teleport";
        if (!c1AI.los.visibleEnemy.Contains(c1AI.Player))
        {
            c1AI.gameObject.transform.position = GetNewTeleportWayPoint(c1AI);
        }
    }

    public override void UpdateState(Commander1AI c1AI)
    {
        if (!c1AI.los.visibleEnemy.Contains(c1AI.Player))
        {
            GetVisiblePoints(c1AI);
            Vector3 teleportWaypoint = GetNewTeleportWayPoint(c1AI);
            if (c1AI.gameObject.transform.position != teleportWaypoint)
            {
                
                c1AI.gameObject.transform.position = teleportWaypoint;
            }
            else
            {
                c1AI.SwitchState(c1AI.jump);
            }
            visiblePoints.Clear();
        }
    }

    private Vector3 GetNewTeleportWayPoint(Commander1AI c1AI)
    {
        Vector3 bestTeleportPoint = c1AI.teleportLocations[Random.Range(0, c1AI.teleportLocations.Count)].position;
        float distance = Mathf.Infinity;
        foreach (Transform location in visiblePoints) 
        {
            if (Vector3.Distance(location.position, c1AI.Player.transform.position) < distance)
            {
                distance = Vector3.Distance(location.position, c1AI.Player.transform.position);
                bestTeleportPoint = location.position;
            }
        }
        return bestTeleportPoint;
    }

    private void GetVisiblePoints(Commander1AI c1AI)
    {
        foreach (Transform location in c1AI.teleportLocations)
        {
            Ray r = new();
            r.origin = location.position;
            r.direction = location.position - c1AI.Player.transform.position;
            if (!Physics.Raycast(r, Mathf.Infinity, c1AI.los.obstructLayer))
            {
                visiblePoints.Add(location);
            }
        }
    }
}