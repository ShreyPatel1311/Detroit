using System.Collections.Generic;
using UnityEngine;

public class CoverState : AUState
{
    private CoverSpot bestCover;
    public override void EnterState(ArmyUnitAI auAI)
    {
        this.name = "cover";
        auAI.anim.SetLayerWeight(1, 1);
        auAI.anim.SetBool("Sprint", true);
        auAI.nav.speed = 3;
    }

    public override void UpdateState(ArmyUnitAI auAI)
    {
        if(Vector3.Distance(auAI.gameObject.transform.position, auAI.player.transform.position) < auAI.ChaseRadius)
        {
            if(bestCover == null)
            {
                FindCover(auAI);
            }
            auAI.anim.SetBool("Crouch", Vector3.Distance(auAI.transform.position, bestCover.transform.position) <= auAI.nav.stoppingDistance);
            if(Vector3.Distance(auAI.player.transform.position, bestCover.transform.position) <= auAI.AlertRadius)
            {
                auAI.nav.SetDestination(bestCover.transform.position);
            }
            else
            {
                auAI.SwitchState(auAI.Ofs);
            }
        }
        else
        {
            auAI.SwitchState(auAI.Chase);
        }
    }

    //private bool AmICovered(ArmyUnitAI auAI)
    //{ 
    //    if(!Physics.Raycast(auAI.gameObject.transform.position, auAI.gameObject.transform.position - auAI.player.transform.position, Mathf.Infinity, auAI.los.obstructLayer))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private CoverSpot FindCover(ArmyUnitAI auAI)
    {
        List<CoverSpot> visibleSpot = new List<CoverSpot>();
        foreach(CoverSpot coverSpot in auAI.cm.unOccupiedCoverSpots)
        {
            if(!Physics.Raycast(coverSpot.transform.position, coverSpot.transform.position - auAI.player.transform.position, Mathf.Infinity, auAI.los.obstructLayer))
            {
                visibleSpot.Add(coverSpot);
            }
        }

        float distance = Mathf.Infinity;
        bestCover = visibleSpot[0];
        foreach(CoverSpot coverSpot in visibleSpot)
        {
            if(distance < Vector3.Distance(auAI.transform.position, auAI.player.transform.position))
            {
                distance = Vector3.Distance(auAI.transform.position, auAI.player.transform.position);
                bestCover = coverSpot;
            }
        }

        auAI.cm.AddToOccupied(bestCover);
        bestCover.SetOccupier(auAI.gameObject);
        return bestCover;
    }
}