using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [SerializeField] private GameObject Environment2D;
    [SerializeField] private GameObject Environment3D;

    private void Awake()
    {
        instance = this;
    }

    public void ChangePerspective(bool pers)
    {
        if (pers) //2D
        {
            Environment2D.SetActive(true);
            Environment3D.SetActive(false);
        }
        else //3D
        {
            Environment2D.SetActive(false);
            Environment3D.SetActive(true);
        }
    }
}
