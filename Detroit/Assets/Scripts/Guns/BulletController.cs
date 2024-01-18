using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private float speed = 5f;
    public float attackDamage = 10f;

    private float timeToDestroy = 3f;
    private Vector3 dir;
    private Rigidbody rb;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    private void Start()
    {
        dir = (target - transform.position).normalized * speed;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb != null)
        {
            rb.velocity = dir * speed;
        }
        else
        {
            transform.position += new Vector3(dir.x, dir.y, dir.z) * Time.deltaTime;
        }
        if(Vector3.Distance(transform.position, target) < 0.001f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision other)
    { 
        if (other.gameObject.CompareTag("Entity") || other.gameObject.CompareTag("Commander"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
        Destroy(gameObject);
    }
}
