using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckpointManager : MonoBehaviour
{
    public GameObject checkpointPrefab;
    public GameObject checkpointCrossEffect;

    public Transform[] checkpointPositions;
    public TextMeshPro checkpointText;
    public Transform directionalArrow;
    public float rotationSpeed = 5f;

    public AudioSource audioSource; 
    public AudioClip checkpointCrossClip; 

    private int checkpointsCrossed = 0;

    void Start()
    {
        SpawnCheckpoints();
        UpdateCheckpointText();
        UpdateArrowDirection();
        checkpointCrossEffect.SetActive(false);
        //AudioManager.inst.PlaySound(SoundName.welcome);
    }

    void SpawnCheckpoints()
    {
        foreach (var position in checkpointPositions)
        {
            Instantiate(checkpointPrefab, position.position, position.rotation);
        }
    }

    public void CrossCheckpoint()
    {
        if (checkpointsCrossed < checkpointPositions.Length)
        {
            checkpointsCrossed++;
            UpdateCheckpointText();
            UpdateArrowDirection();
            PlayCheckpointCrossSound();

            if (checkpointsCrossed == checkpointPositions.Length)
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
            checkpointText.text = $"{checkpointsCrossed}/{checkpointPositions.Length}";
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
        if (directionalArrow != null && checkpointsCrossed < checkpointPositions.Length)
        {
            Vector3 targetPosition = checkpointPositions[checkpointsCrossed].position;

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
