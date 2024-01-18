using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject go;
    [SerializeField] private float health = 100f;
    private GameManager gm;

    public void TakeDamage(float value)
    {
        health -= value;
        gm = go.GetComponent<GameManager>();
    }

    public void GainHealth(float value)
    {
        health += value;
    }

    private void Update()
    {
        if(health <= 0f)
        {
            gm.gameOver = true;
        }
    }
}
