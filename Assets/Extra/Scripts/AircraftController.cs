using System.Collections;
using Oyedoyin.Common;
using UnityEngine;

public class AircraftController : MonoBehaviour
{
    public Rigidbody aircraftRigidbody;
    public Controller aircraftController;
    private CheckpointManager checkpointManager;

    // Canvas
    public Canvas crashScreen;
    public GameObject fadeOutPanel;

    // Landing 
    private bool isLanded = false;
    private bool landingTimerStarted = false;
    public float landingTimeThreshold = 5f;
    private float landingTimer = 0f;

    // Airborne 
    private bool isAirborne = false;
    private float airborneTime = 0f;
    public float requiredAirborneTime = 30f; 

    private void Start()
    {
        crashScreen.enabled = false;
        fadeOutPanel.SetActive(false);

        DontDestroyOnLoad(crashScreen.gameObject);
        DontDestroyOnLoad(fadeOutPanel.gameObject);

        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void Update()
    {
        if (isAirborne && !isLanded)
        {
            airborneTime += Time.deltaTime;
        }

        if (isLanded && landingTimerStarted)
        {
            landingTimer += Time.deltaTime;

        
            if (landingTimer >= landingTimeThreshold)
            {
                SwitchToWinCamera();
            }
        }
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
        // If the collision is strong enough to be considered a crash
        if (collision.relativeVelocity.magnitude > aircraftController.crashThreshold)
        {
            HandleFlightCrash(aircraftRigidbody, aircraftController);
        }

        else if (collision.gameObject.CompareTag("Ground"))
        {
            //cond for checking the air time 
            if (airborneTime >= requiredAirborneTime)
            {
                isLanded = true;
                StartLandingTimer();
            }
            else
            {
                Debug.Log("Aircraft tried to land before being in the air for the required time.");
            }

            // Reset airborne 
            isAirborne = false;
            airborneTime = 0f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //cond for flying
        if (collision.gameObject.CompareTag("Ground"))
        {
            isAirborne = true;
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
            controller.isCrashed = true;

            if (controller.gearActuator != null && controller.gearActuator.actuatorState == SilantroActuator.ActuatorState.Engaged)
            {
                controller.gearActuator.DisengageActuator();
            }

            StartCoroutine(LoadSceneCoroutine(2));
        }
    }

    private void StartLandingTimer()
    {
        if (!landingTimerStarted)
        {
            landingTimerStarted = true;
            landingTimer = 0f;
        }
    }

    private void SwitchToWinCamera()
    {
        landingTimerStarted = false;
        CameraController.instance.SwitchToCamera(CameraType.winCam);
        Debug.Log("Switched to Win Camera after successful landing");
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
