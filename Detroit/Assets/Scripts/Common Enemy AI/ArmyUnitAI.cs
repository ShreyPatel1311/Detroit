using UnityEngine;
using UnityEngine.AI;

public class ArmyUnitAI : MonoBehaviour
{
    [SerializeField] private float alertRadius = 20f;
    [SerializeField] private float chaseRadius = 2f;
    [SerializeField] private float coverRadius = 5f;

    private AUState currentState;
    private PatrolState patrol = new();
    private ChaseState chase = new();
    private CoverState cover = new();
    private OpenFireState ofs = new();

    public float AlertRadius { get => alertRadius; }
    public float ChaseRadius { get => chaseRadius; }
    public float CoverRadius { get => coverRadius; }
    public PatrolState Patrol { get => patrol; }
    public ChaseState Chase { get => chase; }
    public CoverState Cover { get => cover; }
    public OpenFireState Ofs { get => ofs; }

    [HideInInspector] public Transform spawnLocations;
    [HideInInspector] public Animator anim;
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public CapsuleCollider cc;
    [HideInInspector] public LineOfSight los;
    [HideInInspector] public GameObject player;
    [HideInInspector] public CoverManager cm;

    // Start is called before the first frame update
    void Start()
    {
        cm = Camera.main.GetComponent<CoverManager>();
        los = GetComponent<LineOfSight>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        cc = GetComponent<CapsuleCollider>();
        currentState = patrol;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    { 
        currentState.UpdateState(this);
        Debug.Log(spawnLocations.name + ":" + currentState.name);
    }

    public void SwitchState(AUState aus)
    {
        currentState = aus;
        currentState.EnterState(this);
    }
}