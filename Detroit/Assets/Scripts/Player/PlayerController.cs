using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private PlayerInput pi;
    private InputAction MoveAction;
    private InputAction JumpAction;
    private InputAction shootAction;
    private InputAction crouchAction;
    private InputAction dashAction;
    private Transform CameraTransform;
    private AudioSource audio1;
    private bool crouching;
    private LineOfSight los;
    private bool shooting;
    private Gun gun;

    [Header("Guns")]
    [SerializeField] private GameObject assaultRifle;

    [Header("Magic Values")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float bulletMissDis = 25f;
    [SerializeField] private float animTransition = 0.15f;

    [Header("World Objects")]
    [SerializeField] private GameObject refrenceLOS;
    [SerializeField] private Animator anim;
    [SerializeField] private SwitchVCam svc;
    [SerializeField] private AudioClip walkClip;

    private void Awake()
    {
        audio1 = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        pi = GetComponent<PlayerInput>();
        CameraTransform = Camera.main.transform;
        MoveAction = pi.actions["Move"];
        dashAction = pi.actions["Dash"];
        crouchAction = pi.actions["Crouch"];
        JumpAction = pi.actions["Jump"];
        shootAction = pi.actions["Shoot"];
        gun = assaultRifle.GetComponent<Gun>();

        los = refrenceLOS.GetComponent<LineOfSight>();
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponentInChildren<Animator>();
    }

    private void Ready() 
    {
        svc.isAiming = true;
    }

    private void NotReady() {
        svc.isAiming = false;
    }

    private void ShootGun()
    {
        gun.Fire(svc, CameraTransform, anim);
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = MoveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * CameraTransform.right.normalized + move.z * CameraTransform.forward.normalized;
        move.y = 0f;

        if (crouchAction.IsPressed() && !crouching)
        {
            crouching = true;
        }
        if(Input.GetKey("left shift") || JumpAction.triggered)
        {
            crouching = false;
        }
        if (svc.isAiming)
        {
            anim.SetLayerWeight(2, 1);
            move /= 2;
        }
        else
        {
            anim.SetLayerWeight(2, 0);
        }

        if (crouching)
        {
            anim.SetLayerWeight(3, 1);
            anim.SetBool("Crouch", true);
            move /= 2;
        }
        else
        {
            anim.SetLayerWeight(3, 0);
            anim.SetBool("Crouch", false);
        }

        controller.Move(move * Time.deltaTime * playerSpeed);
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveZ", input.y);

        if (JumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.Play("Jump_Up");
        }

        if (MoveAction.IsInProgress() && !GetComponent<AudioSource>().isPlaying)
        {
            audio1.PlayOneShot(walkClip);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (MoveAction.IsInProgress() || svc.isAiming)
        {
            Quaternion rotation = Quaternion.Euler(0, CameraTransform.eulerAngles.y, 0);
            transform.rotation = Quaternion.LookRotation(new Vector3(CameraTransform.forward.x, 0, CameraTransform.forward.z), Vector3.up);
        }

        if (shootAction.IsInProgress())
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }

        if (shooting)
        {
            ShootGun();
        }

        if (dashAction.triggered)
        {
            GameObject dashTarget = GetDashTarget();
            transform.rotation = Quaternion.LookRotation(dashTarget.transform.position - transform.position, Vector3.up);
            transform.position = Vector3.MoveTowards(transform.position, dashTarget.transform.position, Vector3.Distance(transform.position, GetDashTarget().transform.position));
            dashTarget.GetComponent<EnemyHealth>().selectedDashTarget = true;
        }
    }

    private GameObject GetDashTarget()
    {
        Ray ray = new Ray(transform.position, CameraTransform.forward);
        float distance = Mathf.Infinity;
        GameObject selectedObject = los.visibleEnemy[0];
        foreach (GameObject dashTarget in los.visibleEnemy)
        {
            if (!dashTarget.GetComponent<EnemyHealth>().selectedDashTarget)
            {
                Vector3 nearestPointOnRay = ray.GetPoint(Vector3.Dot(dashTarget.transform.position - ray.origin, ray.direction));
                float dashDistance = Vector3.Distance(dashTarget.transform.position, nearestPointOnRay);

                if (dashDistance < distance)
                {
                    distance = dashDistance;
                    selectedObject = dashTarget;
                }
            }
        }
        return selectedObject;
    }
}