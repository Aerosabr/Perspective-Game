using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool isActive;
    private GameObject Button;
    [SerializeField] private GameObject Elevator;

    private void Start()
    {
        isActive = false;
        Button = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        if (isActive && Elevator.transform.position.y > 5)
        {
            Elevator.transform.position = Vector3.MoveTowards(Elevator.transform.position, new Vector3(0, 5, 0), 10 * Time.deltaTime);
        }
        else if (!isActive && Elevator.transform.position.y < 20)
        {
            Elevator.transform.position = Vector3.MoveTowards(Elevator.transform.position, new Vector3(0, 20, 0), 2 * Time.deltaTime);

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            Debug.Log("Active");
            isActive = true;
            Button.transform.localPosition = new Vector3(0, -0.1f, 0);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isActive)
        {
            Debug.Log("Not active");
            isActive = false;
            Button.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
