using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager inst;
    public SoundName[] tutorialSequence;
    private int currentStepIndex = 0;

    public void Awake()
    {
        inst = this;
    }

    public void Sequence()
    {
        Debug.Log("In sequence");
        if (tutorialSequence.Length > 0)
        {
            PlayNextTutorialStep();
        }
    }

    void PlayNextTutorialStep()
    {
        Debug.Log("current step index : "+ currentStepIndex);
        if (currentStepIndex < tutorialSequence.Length)
        {
            SoundName audioToPlay = tutorialSequence[currentStepIndex];
            Debug.Log("SoundName : " + audioToPlay);

            AudioManager.inst.PlaySound(audioToPlay);

            StartCoroutine(WaitForAudioToFinish());
        }
    }

    IEnumerator WaitForAudioToFinish()
    {
        while (AudioManager.inst.IsAudioPlaying())
        {
            yield return null;  
        }
        yield return new WaitForSeconds(1.5f);
        ProceedToNextStep();
    }

    void ProceedToNextStep()
    {
        Debug.Log("ProceedToNextStep called!");
        currentStepIndex++;
        PlayNextTutorialStep();
        StopAudio();
    }

    void StopAudio()
    {
        if (currentStepIndex == tutorialSequence.Length)
        {
            AudioSource[] audioSources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].UnPause();
            }
        }
    }
}
