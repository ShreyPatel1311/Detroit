using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Gun
{
    private int bulletCount;
    private float timer = 0f;

    [SerializeField] private float bulletMissDis;
    [SerializeField] private float animTransition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float timeBetweenEachFire = 0.1f;

    public override void Fire(SwitchVCam svc, Transform CameraTransform, Animator anim)
    {
        RaycastHit hit;
        timer += Time.deltaTime;
        if (svc.isAiming && bulletCount <= 30 && timer >= timeBetweenEachFire)
        {
            GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.LookRotation(-CameraTransform.forward), bulletParent);
            BulletController bullCon = bullet.GetComponent<BulletController>();
            GetComponent<AudioSource>().PlayOneShot(fireClip);
            bulletCount++;
            if (Physics.Raycast(CameraTransform.position, CameraTransform.forward, out hit, Mathf.Infinity))
            {
                bullCon.target = hit.point;
                bullCon.hit = true;
            }
            else
            {
                bullCon.target = CameraTransform.position + CameraTransform.forward * bulletMissDis;
                bullCon.hit = false;
            }
            timer = 0f;
        }
        if (bulletCount > 30)
        {
            anim.CrossFade("Reload", animTransition);
            bulletCount = 0;
        }
    }
}
