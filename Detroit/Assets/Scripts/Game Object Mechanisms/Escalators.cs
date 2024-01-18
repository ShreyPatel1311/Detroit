using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalators : MonoBehaviour
{

    [SerializeField] private Transform finalPosition;
    [SerializeField] private Transform firstPosition;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;  

    private float timer = 0f;
    private Vector3 target;

    private void Start()
    {
        target = finalPosition.position;
    }

    private void Update()
    {
        if(Vector3.Distance(this.gameObject.transform.position, finalPosition.position) <= 0.1f)
        {
            timer += Time.deltaTime;
            if(timer > waitTime)
            {
                timer = 0f;
                target = firstPosition.position;
            }
        }
        else if(Vector3.Distance(this.gameObject.transform.position, firstPosition.position) <= 0.1f)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                timer = 0f;
                target = finalPosition.position;
            }
        }
        transform.position = Vector3.MoveTowards(this.gameObject.transform.position, target, speed * Time.deltaTime);
    }
}
