using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    [HideInInspector] public List<CoverSpot> unOccupiedCoverSpots = new List<CoverSpot>();
    [HideInInspector] public List<CoverSpot> occupiedCoverSpots = new List<CoverSpot>();
    [HideInInspector] public List<GameObject> allSoldiers = new List<GameObject>();

    private void Awake()
    {
        unOccupiedCoverSpots = new List<CoverSpot>(GameObject.FindObjectsOfType<CoverSpot>());
    }

    public void AddToOccupied(CoverSpot spot)
    {
        if (unOccupiedCoverSpots.Contains(spot))
        {
            unOccupiedCoverSpots.Remove(spot);
        }
        if (!occupiedCoverSpots.Contains(spot))
        {
            occupiedCoverSpots.Add(spot);
        }
    }
    public void AddToUnoccupied(CoverSpot spot)
    {
        if (occupiedCoverSpots.Contains(spot))
        {
            occupiedCoverSpots.Remove(spot);
        }
        if (!unOccupiedCoverSpots.Contains(spot))
        {
            unOccupiedCoverSpots.Add(spot);
        }
    }

    public CoverSpot GetCoverTowardsTarget(GameObject soldier, Vector3 targetPosition, float minAttackDistance)
    {
        CoverSpot bestCover = null;
        Vector3 soldierPosition = soldier.transform.position;
        CoverSpot[] possibleCoverSpots = unOccupiedCoverSpots.ToArray();

        for (int i = 0; i < possibleCoverSpots.Length; i++)
        {
            CoverSpot spot = possibleCoverSpots[i];

            if (!spot.IsOccupied() && spot.AmICoveredFrom(targetPosition) && Vector3.Distance(spot.transform.position, targetPosition) >= minAttackDistance)
            {
                if (bestCover == null)
                {
                    bestCover = spot;
                }
                else if (Vector3.Distance(bestCover.transform.position, soldierPosition) > Vector3.Distance(spot.transform.position, soldierPosition) && Vector3.Distance(spot.transform.position, targetPosition) < Vector3.Distance(soldierPosition, targetPosition))
                {
                    if (Vector3.Distance(spot.transform.position, soldierPosition) < Vector3.Distance(targetPosition, soldierPosition))
                    {
                        bestCover = spot;
                    }
                }
            }
            if(spot.GetOccupier() == null)
            {
                AddToUnoccupied(spot);
            }
        }

        if (bestCover != null)
        {
            bestCover.SetOccupier(soldier);
            AddToOccupied(bestCover);
        }

        return bestCover;
    }

    public void ExitCover(CoverSpot spot)
    {
        if (spot != null)
        {
            spot.SetOccupier(null);
            AddToUnoccupied(spot);
        }
    }
}