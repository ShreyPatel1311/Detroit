using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMechanism : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    private Vector3 initialLeftDoorTrans;
    private Vector3 initialRightDoorTrans;

    private void Start()
    {
        initialLeftDoorTrans = leftDoor.transform.position;
        initialRightDoorTrans = rightDoor.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        leftDoor.transform.position = initialLeftDoorTrans;
        rightDoor.transform.position = initialRightDoorTrans;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            leftDoor.transform.Translate(Vector3.left * 2f);
            rightDoor.transform.Translate(Vector3.right * 2f);
        }
    }
}
