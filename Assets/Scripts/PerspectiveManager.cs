using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static PerspectiveManager instance;
    public GameObject Player;

    private void Awake()
    {
        instance = this;
    }

    public void SwitchPerspective()
    {
        
    }
}
