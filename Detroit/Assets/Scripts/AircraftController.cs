using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [Header("Speed Variables")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float hoverSpeed;
    [SerializeField] private float lookRateSpeed = 90f;
    [SerializeField] private float rollSpeed = 90f;

    [Header("Acceleration Variables")]
    [SerializeField] private float forwardAcc = 2.5f;
    [SerializeField] private float strafeAcc = 2f;
    [SerializeField] private float hoverAcc = 2f;
    [SerializeField] private float rollAcc = 3.5f;

    private Vector2 lookInput, screenCenter, mouseDistance;
    private float activeForwardSpeed;
    private float activeHoverSpeed;
    private float activeStrafeSpeed;
    private float rollInput;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance = new Vector2((lookInput.x - screenCenter.x)/screenCenter.y, (lookInput.y - screenCenter.y)/screenCenter.y);
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcc * Time.deltaTime);

        transform.Rotate(mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcc * Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcc * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcc * Time.deltaTime);
        transform.position += (-transform.forward * activeForwardSpeed * Time.deltaTime) + (-transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);
    }
}
