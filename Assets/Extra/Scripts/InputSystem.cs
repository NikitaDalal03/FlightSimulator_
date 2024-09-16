using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputSystem : MonoBehaviour
{
    public InputActionReference menuAction;
    public InputActionReference restartAction;
    public InputActionReference playAction;

    private void OnEnable()
    {
        playAction.action.performed += OnPlayButtonPressed;
        playAction.action.Enable();

        menuAction.action.performed += OnMenuButtonPressed;
        menuAction.action.Enable();

        restartAction.action.performed += OnRestartButtonPressed;
        restartAction.action.Enable();
    }

    private void OnDisable()
    {
        playAction.action.performed -= OnPlayButtonPressed;
        playAction.action.Disable();

        menuAction.action.performed -= OnMenuButtonPressed;
        menuAction.action.Disable();

        restartAction.action.performed -= OnRestartButtonPressed;
        restartAction.action.Disable();
    }

   

    public void OnPlayButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("X pressed");
        //AudioManager.inst.Source.Play();
        TutorialManager.inst.Sequence();
        CameraController.instance.SwitchToCamera(CameraType.vrCam);

        //SceneManager.LoadScene(1);
    }
    
    private void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        CameraController.instance.SwitchToCamera(CameraType.startCam);

        //SceneManager.LoadScene(0);
    }

    private void OnRestartButtonPressed(InputAction.CallbackContext context)
    {
        CameraController.instance.SwitchToCamera(CameraType.vrCam);

        //SceneManager.LoadScene(1);
    }
}