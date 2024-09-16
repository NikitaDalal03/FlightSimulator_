using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{

    public GameObject checkpointCrossEffect;


    public TextMeshPro checkpointText;
    public Transform directionalArrow;
    public float rotationSpeed = 5f;

    public AudioSource audioSource; 
    public AudioClip checkpointCrossClip; 

    private int checkpointsCrossed = 0;

    void Start()
    {

        UpdateCheckpointText();
        UpdateArrowDirection();
        checkpointCrossEffect.SetActive(false);
        //AudioManager.inst.PlaySound(SoundName.welcome);
    }


    public void CrossCheckpoint()
    {
        if (checkpointsCrossed < CheckpointSpawning.instance.checkpointPositions.Length)
        {
            checkpointsCrossed++;
            UpdateCheckpointText();
            UpdateArrowDirection();
            PlayCheckpointCrossSound();

            if (checkpointsCrossed == CheckpointSpawning.instance.checkpointPositions.Length)
            {
                //SceneManager.LoadScene(1);
                CameraController.instance.SwitchToCamera(CameraType.vrCam);
            }
        }
    }

    void UpdateCheckpointText()
    {
        if (checkpointText != null)
        {
            checkpointText.text = $"{checkpointsCrossed}/{CheckpointSpawning.instance.checkpointPositions.Length}";
            StartCoroutine(ActivateCheckpointEffect());
        }
    }

    IEnumerator ActivateCheckpointEffect()
    {
        checkpointCrossEffect.SetActive(true);
        yield return new WaitForSeconds(4f); 
        checkpointCrossEffect.SetActive(false);
    }

    void UpdateArrowDirection()
    {
        if (directionalArrow != null && checkpointsCrossed < CheckpointSpawning.instance.checkpointPositions.Length)
        {
            Vector3 targetPosition = CheckpointSpawning.instance.checkpointPositions[checkpointsCrossed].position;

            // Calculate the direction to the checkpoint 
            Vector3 direction = (targetPosition - directionalArrow.position).normalized;

            // Calculate the target rotation to face the checkpoint 
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            directionalArrow.rotation = Quaternion.Slerp(directionalArrow.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        UpdateArrowDirection();
    }

    void PlayCheckpointCrossSound()
    {
        if (audioSource != null && checkpointCrossClip != null)
        {
            audioSource.PlayOneShot(checkpointCrossClip); 
        }
    }
}
