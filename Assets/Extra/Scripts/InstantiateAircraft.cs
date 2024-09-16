using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAircraft : MonoBehaviour
{
    public static InstantiateAircraft instance;
    public GameObject aircraftPrefab;
    private GameObject currentAircraft;

    public Transform spawnPosition;

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

        InstantiatePlane();
    }

    public void InstantiatePlane()
    {
        if (currentAircraft != null)
        {
            Destroy(currentAircraft);
        }

        currentAircraft = Instantiate(aircraftPrefab, spawnPosition.position, spawnPosition.rotation);
        currentAircraft.SetActive(true);
    }

    public void OnDestroy()
    {
 
        if (currentAircraft != null)
        {
            Destroy(currentAircraft);
            currentAircraft = null;
        }
    }
}
