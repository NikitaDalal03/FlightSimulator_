using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetails : MonoBehaviour
{
    public GameObject cameraGameObject;  
    public CameraType cameraType;
    public Canvas canvas;

    public void EntryView()
    {
        if (cameraGameObject != null)
        {
            cameraGameObject.SetActive(true);  
        }

        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.gameObject.SetActive(true);
        }
    }

    public void ExitView()
    {
        if (cameraGameObject != null)
        {
            cameraGameObject.SetActive(false);
        }

        if (canvas != null)
        {
            canvas.enabled = false;
            canvas.gameObject.SetActive(false);
        }
    }
}

public enum CameraType
{
    startCam,
    menuCam,
    vrCam,
    None
}