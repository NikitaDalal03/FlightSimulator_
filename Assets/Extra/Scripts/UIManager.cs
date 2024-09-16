using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button startBtn;
    public Button restartBtn;
    public Button menuBtn;


    [ContextMenu("Start Game")]
    public void OnStart()
    {
        Debug.Log("OnStartFunction");
        //SceneManager.LoadScene(1);
        TutorialManager.inst.Sequence();
        CameraController.instance.SwitchToCamera(CameraType.vrCam);
    }

    [ContextMenu("Resart Game")]
    public void OnRestart()
    {
        Debug.Log("OnRestartFunction");
        //SceneManager.LoadScene(1);
        CameraController.instance.SwitchToCamera(CameraType.vrCam);
    }

    public void OnMenu()
    {   
        Debug.Log("OnMenuFunction");
        //SceneManager.LoadScene(0); 
        CameraController.instance.SwitchToCamera(CameraType.startCam);
    }
}