using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private PlayerInput pi;
    [SerializeField] private int PriorityBoost = 10;
    [SerializeField] private Canvas TrdPersonCanvas;
    [SerializeField] private Canvas AimCanvas;
    [SerializeField] private Animator anim;

    private CinemachineVirtualCamera VCam;
    private InputAction aimAction;
    public bool isAiming { get; set; }

    private void Awake()
    {
        VCam = GetComponent<CinemachineVirtualCamera>();
        aimAction = pi.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        VCam.Priority += PriorityBoost;
        TrdPersonCanvas.enabled = false;
        AimCanvas.enabled = true;
        anim.SetBool("Aim", true);
        isAiming = true;
    }

    private void CancelAim()
    {
        VCam.Priority -= PriorityBoost;
        TrdPersonCanvas.enabled = true;
        AimCanvas.enabled = false;
        anim.SetBool("Aim", false);
        isAiming = false;
    }
}
