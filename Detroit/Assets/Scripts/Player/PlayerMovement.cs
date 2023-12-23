using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Env Charac.")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 1.0f;

    [Header("input System Variables")]
    [SerializeField] private PlayerInput pInp;
    private InputAction look;
    private InputAction move;
    private InputAction shoot;
    private InputAction jump;

    private Animator anim;
    private CharacterController cc;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        pInp = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        look = pInp.actions["Look"];
        move = pInp.actions["Move"];
        shoot = pInp.actions["Shoot"];
        jump = pInp.actions["Jump"];
    }

    void Update()
    {
        groundedPlayer = cc.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = move.ReadValue<Vector2>();
        Vector3 motion = new Vector3(input.x, 0, input.y);
        motion = motion.x * Camera.main.transform.right.normalized + motion.z * Camera.main.transform.forward.normalized;
        motion.y = 0f;
        cc.Move(motion * Time.deltaTime * playerSpeed);

        if (move.IsInProgress())
        {
            anim.SetBool("Move", true);
            anim.SetFloat("walkX", input.x);
            anim.SetFloat("walkY", input.y);
        }
        else
        {
            anim.SetBool("Move", false);
        }

        // Changes the height position of the player..
        if (jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (look.triggered)
        {
            Quaternion rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(rotation, transform.rotation, rotationSpeed * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
}