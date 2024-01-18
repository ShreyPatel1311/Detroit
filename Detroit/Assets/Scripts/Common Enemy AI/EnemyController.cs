using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float spotWaitTime = 5.0f;
    [SerializeField] private LayerMask hideableMask;
    [SerializeField] private float minAttackDistance = 6f;
    [SerializeField] private float maxAttackdistance = 25f;
    [SerializeField] private float chaseRadius = 40f;

    public bool spotted;
    [HideInInspector] public GameObject mainCamera;
    [HideInInspector] public bool covered;

    private CoverManager cm;
    private LineOfSight los;
    private CapsuleCollider cc;
    private RandomPatrol rp;
    private EnemyAttack ea;
    private CoverSpot currSpot;
    private NavMeshAgent nav;
    private Animator anim;
    private float timer = 0f;
    private GameObject player;

    private void Awake()
    {
        mainCamera = Camera.main.gameObject;
        cc = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        covered = false;
        anim = GetComponent<Animator>();
        cm = mainCamera.GetComponent<CoverManager>();
        los = GetComponent<LineOfSight>();
        rp = GetComponent<RandomPatrol>();
        nav = GetComponent<NavMeshAgent>();
        ea = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        SpotFollow();
    }

    private void SpotFollow()
    {
        if (los.visibleEnemy.Contains(player))
        {
            anim.SetBool("Sprint", true);
            rp.enabled = false;
            timer += Time.deltaTime;
            los.viewAngle = 360f;
            if (timer >= spotWaitTime && spotted)
            {
                if (currSpot == null)
                {
                    currSpot = Hide(player.transform);
                }
            }
            else
            {
                spotted = true;                         //future plans ???!!!!
            }
            if(currSpot != null)
            {
                nav.speed = 3f;
                if(Vector3.Distance(transform.position, currSpot.transform.position) <= nav.stoppingDistance)
                {
                    SetHeight(1.1f);
                    anim.SetLayerWeight(1, 0);
                    anim.SetBool("Crouch", true);
                    transform.LookAt(player.transform.position);
                    Fire();
                }
                else
                {
                    transform.LookAt(currSpot.transform.position);
                    ea.enabled = false;
                }
                nav.SetDestination(currSpot.transform.position);
                rp.SynchronizeAnimatorAndNavMesh(1, 1);
            }
        }
        else
        {
            timer = 0f;
            rp.SynchronizeAnimatorAndNavMesh(1, 0);
            anim.SetBool("Crouch", false);
            los.viewAngle = 110f;
            ea.enabled = false;
            covered = false;
            nav.speed = 1f;
        }
        if (spotted && !los.visibleEnemy.Contains(player))
        {
            if (!Physics.Raycast(currSpot.transform.position, currSpot.transform.position - player.transform.position, Mathf.Infinity, hideableMask))
            {
                rp.enabled = false;
                if(currSpot != null)
                    cm.ExitCover(currSpot);
                currSpot = Hide(player.transform);
                nav.SetDestination(currSpot.transform.position);

            }
        }
        if(Vector3.Distance(transform.position, player.transform.position) >= chaseRadius)
        {
            spotted = false;
            ea.enabled = false;
            rp.enabled = true;
        }
    }

    private void SetHeight(float value)
    {
        nav.height = value;
        cc.height = value;
    }

    public CoverSpot Hide(Transform target)
    {
        nav.isStopped = false;
        anim.SetBool("Crouch", false);
        SetHeight(1.8f);
        CoverSpot bestCover = cm.unOccupiedCoverSpots[0];
        float distance = Mathf.Infinity;
        foreach(CoverSpot current in cm.unOccupiedCoverSpots)
        {
            Vector3 dir = current.transform.position - target.position;
            if (!Physics.Raycast(current.transform.position, dir, Mathf.Infinity, hideableMask) && distance < Vector3.Distance(current.transform.position, target.position))
            {
                bestCover = current;
            }
        }
        bestCover.SetOccupier(target.gameObject);
        cm.AddToOccupied(bestCover);
        return bestCover;
    }

    private void Fire()
    { 
        Vector3 coverToThis = currSpot.transform.position - this.transform.position;
        Vector3 playerToThis = player.transform.position - this.transform.position;
        if (covered || Vector3.Dot(coverToThis, playerToThis) > 0)
        {
            ea.enabled = true;
        }
    }
}