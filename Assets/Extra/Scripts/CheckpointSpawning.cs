using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawning : MonoBehaviour
{
    public static CheckpointSpawning instance;
    public GameObject checkpointPrefab;
    public Transform[] checkpointPositions;

    private List<GameObject> spawnedCheckpoints = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SpawnCheckpoints();
    }

    public void SpawnCheckpoints()
    {
        foreach (var position in checkpointPositions)
        {
            GameObject checkpoint = Instantiate(checkpointPrefab, position.position, position.rotation);
            spawnedCheckpoints.Add(checkpoint);
        }
    }

    public void DestroyPreviousCheckpoints()
    {
        foreach (GameObject checkpoint in spawnedCheckpoints)
        {
            Destroy(checkpoint);
        }

        spawnedCheckpoints.Clear();
    }
}
