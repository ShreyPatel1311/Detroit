using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private List<Transform> barrelTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bullShotGap = 0.5f;
    [SerializeField] private float aimFactor = 0.25f;
    [SerializeField] private AudioClip plasmaFireClip;
    [SerializeField] private Rig aimRig;
    [SerializeField] private List<GameObject> gun;
    [SerializeField] private Vector3 offsetFire;

    public GameObject bulletParent;
    public float timeBetweenAttacks = 3f;
    private AudioSource audio1;
    private float timer = 0;
    private GameObject target;
    private bool prevShoot;
    private int x = 0;
    private MultiAimConstraint mac;
    private WeightedTransform wTarget;

    private void OnEnable()
    {
        aimRig.weight = 1.0f;
    }

    private void OnDisable()
    {
        aimRig.weight = 0f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        mac = GetComponent<MultiAimConstraint>();
        audio1 = GetComponent<AudioSource>();
        target = GameObject.Find("Player");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        wTarget.transform = target.transform;
        Debug.DrawRay(barrelTransform[x].position, barrelTransform[x].forward * 20f, Color.red);
        x = (int)Mathf.Round(Random.Range(0f, barrelTransform.Capacity-1));
        NormalAttack();
        Vector3 player = target.transform.position;
        Vector3 dir = new Vector3(player.x + offsetFire.x, player.y + offsetFire.y, player.z + offsetFire.z) ;
        gun[x].transform.LookAt(dir);
    }

    public void NormalAttack()
    {
        if (timer <= timeBetweenAttacks)
        { 
            if (!prevShoot)
            {
                prevShoot = true;
                Invoke(nameof(Fire), bullShotGap);
            }
        }
        else if(timer > timeBetweenAttacks * 2)
        {
            timer = 0f;
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        audio1.PlayOneShot(plasmaFireClip);
        GameObject bullet = Instantiate(bulletPrefab, barrelTransform[x].position, Quaternion.LookRotation(-barrelTransform[x].forward), bulletParent.transform);
        BulletController bc = bullet.GetComponent<BulletController>();
        Vector3 dir = new Vector3(barrelTransform[x].forward.x + Random.Range(-aimFactor, aimFactor), barrelTransform[x].forward.y + Random.Range(-aimFactor, aimFactor), barrelTransform[x].forward.z + Random.Range(-aimFactor, aimFactor));
        if (Physics.Raycast(barrelTransform[x].position, dir, out hit, Mathf.Infinity))
        {
            bc.target = hit.point;
            bc.hit = true;
        }
        prevShoot = false;
        aimRig.weight = 1f;
    }
}