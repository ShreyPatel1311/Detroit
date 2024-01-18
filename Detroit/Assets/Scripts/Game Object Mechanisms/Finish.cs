using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private GameObject go;
    private GameManager gm;
    private void Start()
    {
        go = Camera.main.gameObject;
        gm = go.GetComponent<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            gm.NextLevel();
        }
    }
}
