using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnLocations = new List<Transform>();
    [SerializeField] private GameObject bulletParent;

    [HideInInspector] public CoverManager cm;

    public bool gameOver;
    public bool playerWon = false;
    private int Count = 0;
    private MultiAimConstraint aimRig;
    private RigBuilder rigBuilder;

    // Start is called before the first frame update
    void Start()
    {
        WeightedTransform player = new WeightedTransform();
        player.transform = GameObject.Find("Player").transform;
        player.weight = 1.0f;
        cm = GetComponent<CoverManager>();
        for(int i=0; i<spawnLocations.Capacity; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnLocations[i].position, Quaternion.identity);
            ArmyUnitAI auAI = enemy.GetComponent<ArmyUnitAI>();
            auAI.spawnLocations = spawnLocations[i];
            auAI.player = playerGameObject;

            aimRig = enemy.GetComponentInChildren<MultiAimConstraint>();
            rigBuilder = enemy.GetComponent<RigBuilder>();

            EnemyAttack ea = enemy.GetComponent<EnemyAttack>();
            ea.bulletParent = bulletParent;
            ea.timeBetweenAttacks = Random.Range(4, 7);

            cm.allSoldiers.Add(enemy);
            aimRig.data.sourceObjects.Add(player);
            rigBuilder.Build();
            aimRig.weight = 0;
        }
    }

    private void Update()
    {
        if (playerWon)
        {
            if (Count == 1)
            {
                SceneManager.LoadScene("Level 2");
                playerWon = false;
            }
            else
            {
                SceneManager.LoadScene("Main Menu");
                playerWon = false;
            }
        }
        if (gameOver)
        {
            SceneManager.LoadScene("Main Menu");
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void NextLevel() 
    {
        playerWon = true;
        Count++;
    }
}
