using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    [SerializeField] private List<GameObject> checkpoints;

    public int Checkpoint;

    private void Awake()
    {
        instance = this;
        Checkpoint = 0;
    }

    public Vector3 GetCheckpoint() => checkpoints[Checkpoint].transform.position;
}
