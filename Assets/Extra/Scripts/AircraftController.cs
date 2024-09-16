using System.Collections;
using System.Collections.Generic;
using Oyedoyin.Common;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class AircraftController : MonoBehaviour
{
    public Rigidbody aircraftRigidbody;
    public Controller aircraftController;
    private CheckpointManager checkpointManager;

    // Canvas
    public Canvas crashScreen;
    public GameObject fadeOutPanel;

    private void Start()
    {


        crashScreen.enabled = false;
        fadeOutPanel.SetActive(false);

        DontDestroyOnLoad(crashScreen.gameObject);
        DontDestroyOnLoad(fadeOutPanel.gameObject);

        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            checkpointManager.CrossCheckpoint();
            other.enabled = false;
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > aircraftController.crashThreshold)
        {
            HandleFlightCrash(aircraftRigidbody, aircraftController);
        }
    }

    [ContextMenu("Crash")]
    void Test()
    {
        HandleFlightCrash(aircraftRigidbody, aircraftController);
    }

    public void HandleFlightCrash(Rigidbody rigidbody, Controller controller)
    {
        if (rigidbody != null && controller != null)
        {
            // Deceleration to simulate crash impact 
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            // Downward force to simulate impact if required 
            rigidbody.AddForce(Vector3.down * controller.crashImpactForce, ForceMode.Impulse);

            controller.TurnOffEngines();
            //controller.ToggleBrakeState();

            if (controller.gearActuator != null && controller.gearActuator.actuatorState == SilantroActuator.ActuatorState.Engaged)
            {
                controller.gearActuator.DisengageActuator();
            }

            controller.isCrashed = true;
            
            StartCoroutine(LoadSceneCoroutine(2));
        }
    }

    private IEnumerator LoadSceneCoroutine(int delay)
    {
        yield return new WaitForSeconds(delay);

        crashScreen.enabled = true;
        fadeOutPanel.SetActive(true);
        
        yield return new WaitForSeconds(delay);
        Debug.Log("Crash");

        Destroy(gameObject);
        InstantiateAircraft.instance.OnDestroy();

        //SceneManager.LoadScene(2);
        Canvas menuCanvas = FindObjectOfType<Canvas>();
        if (menuCanvas != null)
        {
            menuCanvas.enabled = true;
        }

        CameraController.instance.SwitchToCamera(CameraType.menuCam);
        InstantiateAircraft.instance.InstantiatePlane();
        CheckpointSpawning.instance.DestroyPreviousCheckpoints();
        CheckpointSpawning.instance.SpawnCheckpoints();

        fadeOutPanel.SetActive(false);
    }


}
