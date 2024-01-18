using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineOfSight), typeof(EnemyAttack), typeof(CharacterController))]
public class Commander1AI : MonoBehaviour
{
    [SerializeField] private float height = 10f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform handRefrence;
    [SerializeField] private float gravityValue = Physics.gravity.y;

    private CommState currentState;
    private bool grenadeThrown;
    private ParticleSystem teleportation;

    [HideInInspector] public CommState teleport = new TeleportState();
    [HideInInspector] public ThrowGrenade throwGre = new ThrowGrenade();
    [HideInInspector] public CommState jump = new JumpState();
    [HideInInspector] public LineOfSight los;
    [HideInInspector] public EnemyAttack ea;
    [HideInInspector] public CharacterController cc;

    public List<Transform> teleportLocations;

    public float Height { get => height; }
    public GameObject Player { get => player; }
    public GameObject GrenadePrefab { get => grenadePrefab; }
    public Transform HandRefrence { get => handRefrence; }
    
    public bool GrenadeThrown { get => grenadeThrown; set => grenadeThrown = value; }
    public ParticleSystem Teleportation { get => teleportation; set => teleportation = value; }

    private void Start()
    {
        los = GetComponent<LineOfSight>();
        ea = GetComponent<EnemyAttack>();
        cc = GetComponent<CharacterController>();
        grenadeThrown = true;
        Teleportation = GetComponentInChildren<ParticleSystem>();
        teleportation.gameObject.SetActive(true);
        currentState = teleport;
        currentState.EnterState(this);
    }

    private void Update()
    {
        if (!cc.isGrounded)
        {
            cc.Move(Vector3.up * gravityValue * Time.deltaTime);
        }
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        Debug.Log(currentState.name) ;
        currentState.UpdateState(this);
    }

    public void SwitchState(CommState cs)
    {
        currentState = cs;
        currentState.EnterState(this);
    }
}
