using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public abstract void Fire(SwitchVCam svc, Transform CameraTransform, Animator anim);
}