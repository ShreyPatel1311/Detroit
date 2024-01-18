using UnityEngine;
using UnityEngine.AI;

public class RandomPatrol : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float range = 10f;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private bool walkPointSet;
    [SerializeField] private float timer = 0f;

    private Vector3 destPoint;
    private NavMeshAgent nav;
    private Animator anim;
    private Vector2 SmoothDeltaPositionMove;
    private Vector2 Velocity;

    [HideInInspector] public Vector3 spawnLocations;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        anim.applyRootMotion = true;
        nav.updatePosition = false;
        nav.updateRotation = true;
        if(spawnLocations == null)
        {
            spawnLocations = transform.position;
        }
    }

    private void Update()
    {
        RandomMove();
    }

    private void RandomMove()
    {
        if (!walkPointSet)
        {
            timer += Time.deltaTime;
            if(timer >= waitTime)
            {
                GetNewWayPoint();
                SynchronizeAnimatorAndNavMesh(0, 1);
            }
        }
        else
        {
            nav.SetDestination(destPoint);  //Walking
            SynchronizeAnimatorAndNavMesh(0, 1);
        }
        if(Vector3.Distance(transform.position, destPoint) <= nav.stoppingDistance)
        {
            walkPointSet = false;           //Stop
        }
    }

    public void SynchronizeAnimatorAndNavMesh(int index, int weight)
    {
        anim.rootPosition = nav.nextPosition;
        Vector3 worldDeltaPosition = nav.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPositionMove = Vector2.Lerp(SmoothDeltaPositionMove, deltaPosition, smooth);

        Velocity = SmoothDeltaPositionMove / Time.deltaTime;

        bool shouldMove = Velocity.magnitude > 0.5f && nav.remainingDistance > nav.stoppingDistance;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            Velocity = Vector2.Lerp(
                Vector2.zero,
                Velocity,
                nav.remainingDistance / nav.stoppingDistance
            );
        }

        anim.SetLayerWeight(index, weight);
        anim.SetBool("move", shouldMove);
        anim.SetFloat("Locomotion", Velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        
        if (deltaMagnitude > nav.radius / 5f)
        {
            transform.position = Vector3.Lerp(
                anim.rootPosition,
                nav.nextPosition,
                smooth
            );
        }
    }

    private void GetNewWayPoint() 
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        destPoint = new Vector3(spawnLocations.x + x, spawnLocations.y, spawnLocations.z + z);

        nav.SetDestination(destPoint);        //Not Walking
        NavMeshHit hit;
        if(NavMesh.SamplePosition(destPoint, out hit, 0.1f, NavMesh.AllAreas))
        {
            timer = 0f;
            walkPointSet = true;
        }
        else
        {
            walkPointSet = false;
        }
    }
}