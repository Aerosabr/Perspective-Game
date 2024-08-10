using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 Position1;
    [SerializeField] private Vector3 Position2;

    [SerializeField] private Vector3 CurrentPos;
    [SerializeField] private float Delay;
    [SerializeField] private float MoveSpeed;

    private int Interval;
    [SerializeField] private bool Stopped;

    private void Awake()
    {
        Interval = 1;
        CurrentPos = Position1;
    }

    private void Update()
    {
        if (Stopped) 
            return;

        if (transform.position == CurrentPos)
        {
            switch (Interval)
            {
                case 1:
                    CurrentPos = Position2;
                    Interval++;
                    break;
                case 2:
                    CurrentPos = Position1;
                    Interval--;
                    break;      
            }
            StartCoroutine(Pause());
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, CurrentPos, Time.deltaTime * MoveSpeed);
    }

    private IEnumerator Pause()
    {
        Stopped = true;
        yield return new WaitForSeconds(Delay);
        Stopped = false;
    }
}
