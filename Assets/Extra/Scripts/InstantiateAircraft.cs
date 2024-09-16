using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAircraft : MonoBehaviour
{
    public static InstantiateAircraft instance;
    public GameObject aircraftPrefab;
    private GameObject currentAircraft;

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

    private void Start()
    {
         //aircraftPrefab.SetActive(false);
         //InstantiatePlane();       
    }

    public void InstantiatePlane()
    {
        if (currentAircraft != null)
        {
            Destroy(currentAircraft);
        }

        currentAircraft = Instantiate(aircraftPrefab);
        currentAircraft.transform.position = aircraftPrefab.transform.position;
        currentAircraft.SetActive(true);

        //var plane = Instantiate(aircraftPrefab);
        //plane.transform.position = aircraftPrefab.transform.position;
        //plane.SetActive(true);
    }

    public void OnDestroy()
    {
        if (currentAircraft != null)
        {
            Destroy(currentAircraft);
            currentAircraft = null;
        }
        //Destroy(aircraftPrefab);
    }
}
