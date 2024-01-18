using UnityEngine;

public class CoverSpot : MonoBehaviour
{
    [SerializeField] private float dotAngle = 0.9f;

    [HideInInspector] public bool occupied = false;
    GameObject occupier;
    Transform cover;

    // Start is called before the first frame update
    void Start()
    {
        cover = transform.parent;
    }

    public void SetOccupier(GameObject occupier)
    {
        this.occupier = occupier;

        if (this.occupier == null)
        {
            occupied = false;
        }
        else
        {
            occupied = true;
        }
    }

    public GameObject GetOccupier()
    {
        return occupier;
    }

    public bool IsOccupied()
    {
        return occupied;
    }

    private void OnDrawGizmos()
    {
        if (IsOccupied())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireSphere(transform.position, 0.5F);
    }

    public bool AmICoveredFrom(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - transform.position;
        Vector3 coverDirection = cover.position - transform.position;

        if (Vector3.Dot(coverDirection, targetDirection) > dotAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AmIBehindTargetPosition(Vector3 soldierPosition, Vector3 targetPosition)
    {
        Vector3 soldierToTargetDirection = targetPosition - soldierPosition;
        Vector3 soldierToCoverDirection = transform.position - soldierPosition;

        float soldierToTargetDistance = Vector3.Distance(soldierPosition, targetPosition);
        float soldierToCoverDistance = Vector3.Distance(soldierPosition, transform.position);

        if ((soldierToCoverDistance + 1) < soldierToTargetDistance)
        {
            return false;
        }

        if (Vector3.Dot(soldierToTargetDirection, soldierToCoverDirection) < 0.7F)
        {
            return false;
        }

        return true;
    }
}