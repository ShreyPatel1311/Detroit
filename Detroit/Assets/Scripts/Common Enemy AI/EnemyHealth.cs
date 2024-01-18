using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent nav;
    private EnemyAttack ea;
    private GameManager gm;
    private AudioSource audio1;
    private ArmyUnitAI auAI;
    private Commander1AI cAI;
    private int count = 0;

    public float health;
    public bool selectedDashTarget;

    // Start is called before the first frame update
    void Start()
    {
        audio1 = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        auAI = GetComponent<ArmyUnitAI>();
        cAI = GetComponent<Commander1AI>();
        gm = Camera.main.GetComponent<GameManager>();   
        health = 100;
    }

    private void Update()
    {
        if (selectedDashTarget && count == 0)
        {
            if (this.gameObject.CompareTag("Entity"))
            {
                TakeDamage(health);
            }
            else
            {
                TakeDamage(50);
            }
        }
    }

    public void TakeDamage(float value)
    {
        health -= value;
        if(health <= 0)
        {
            if(nav != null)
            {
                nav.enabled = false;
            }
            StartCoroutine(RemoveEnemy());
        }
    }

    private IEnumerator RemoveEnemy()
    {
        yield return null;
        if (count == 0)
        {
            anim.SetTrigger("Dead");
            count = 1;
        }
        yield return new WaitForSeconds(3f);
        DisableAllComponents();
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    private void DisableAllComponents()
    {
        audio1.enabled = false;
        anim.enabled = false;
        ea.enabled = false;
        if (this.gameObject.CompareTag("Entity"))
        {
            auAI.enabled = false;
            gm.cm.allSoldiers.Remove(this.gameObject);
        }
        else
        {
            cAI.enabled = false;
        }
    }
}
