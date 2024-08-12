using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    [SerializeField] private List<GameObject> checkpoints;
    [SerializeField] private GameObject Player;

    public int Checkpoint;

    private void Awake()
    {
        instance = this;
        Checkpoint = 0;
    }

    public void GoCheckpoint()
    {
        if (checkpoints[Checkpoint].GetComponent<Checkpoint>().is2D)
        {
            PlayerController player = Player.GetComponent<PlayerController>();
            Player.transform.position = checkpoints[Checkpoint].transform.position;
            player.Perspective2D = true;
            player.transform.GetChild(0).transform.localPosition = new Vector3(.6f, 20, 0);
            player.transform.GetChild(0).transform.rotation = Quaternion.Euler(90, 0, 90);
        }
        else
        {
            PlayerController player = Player.GetComponent<PlayerController>();
            Player.transform.position = checkpoints[Checkpoint].transform.position;
            player.Perspective2D = false;
            player.transform.GetChild(0).transform.localPosition = new Vector3(0, .75f, 0);
            player.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 180, 0);
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
