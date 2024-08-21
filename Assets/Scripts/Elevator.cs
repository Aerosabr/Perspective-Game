using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool isActive;
    public GameObject Player;

    private void Start()
    {
        isActive = false;
    }

    private void Update()
    {
        if (isActive && Mathf.Abs(Player.transform.position.z + 75) * 1.2f > 2.41f)
        {
            //Debug.Log(new Vector3(Player.transform.position.x, Mathf.Abs(Player.transform.position.z + 74f), Player.transform.position.z));
            Player.GetComponent<CharacterController>().enabled = false;
            Player.transform.position = new Vector3(Player.transform.position.x, Mathf.Abs(Player.transform.position.z + 75) * 1.2f, Player.transform.position.z);
            Player.GetComponent<CharacterController>().enabled = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            Debug.Log("Active");
            isActive = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isActive)
        {
            Debug.Log("Not active");
            isActive = false;
        }
    }
}
