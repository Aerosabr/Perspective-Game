using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static PerspectiveManager instance;

    [SerializeField] private bool in2D;
    [SerializeField] private GameObject Player2D;
    [SerializeField] private GameObject Player3D;

    private void Awake()
    {
        instance = this;
    }

    public void SwitchPerspective()
    {
        bool toggle;
        if (in2D)
            toggle = false;
        else
            toggle = true;

        in2D = toggle;
        Player2D.GetComponent<PlayerController>().isActive = toggle;
        Player2D.transform.GetChild(0).gameObject.SetActive(toggle);

        Player3D.GetComponent<PlayerController>().isActive = !toggle;
        Player3D.transform.GetChild(0).gameObject.SetActive(!toggle);
    }
}
