using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Env Charac.")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("input System Variables")]
    [SerializeField] private PlayerInput pInp;
    private InputAction look;
    private InputAction move;
    private InputAction shoot;
    private InputAction jump;
    

    private CharacterController cc;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        pInp = GetComponent<PlayerInput>();
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

        Vector2 motion = move.ReadValue<Vector2>();
        Vector3 moveValue = new Vector3(motion.x, 0, motion.y);
        cc.Move(moveValue * Time.deltaTime * playerSpeed);

        if (moveValue != Vector3.zero)
        {
            gameObject.transform.forward = moveValue;
        }

        // Changes the height position of the player..
        if (jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        if (look.triggered)
        {
            transform.forward = 
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
}