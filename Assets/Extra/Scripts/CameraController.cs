using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public List<CameraDetails> cameraList;
    private Dictionary<CameraType, CameraDetails> cameras = new Dictionary<CameraType, CameraDetails>();

    public CameraType initialCameraType;
    private CameraDetails currentCameraDetails;


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
        InitializeCameras();
        SwitchToCamera(initialCameraType);
    }

    private void InitializeCameras()
    {
        foreach (CameraDetails cameraDetail in cameraList)
        {
            if (!cameras.ContainsKey(cameraDetail.cameraType))
            {
                cameras.Add(cameraDetail.cameraType, cameraDetail);
            }
            else
            {
                Debug.LogWarning("Camera type already exists: " + cameraDetail.cameraType);
            }
        }

        
        foreach (CameraDetails cameraDetail in cameras.Values)
        {
            cameraDetail.ExitView();  
        }
    }

    public void SwitchToCamera(CameraType cameraType)
    {
        if (cameras.ContainsKey(cameraType))
        {
            if (currentCameraDetails != null)
            {
                currentCameraDetails.ExitView();
            }

            currentCameraDetails = cameras[cameraType];
            currentCameraDetails.EntryView();
        }
        else
        {
            Debug.LogWarning("Camera type not found: " + cameraType);
        }
    }    
}

