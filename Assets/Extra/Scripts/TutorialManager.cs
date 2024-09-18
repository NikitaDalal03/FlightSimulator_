/*using System.Collections;
using Oyedoyin.Common;
using Oyedoyin.FixedWing;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager inst;
    public SoundName[] tutorialSequence;
    public GameObject[] tutorialObjects;
    public float highlightDuration = 2f;
    public float maxFresnelPower = 5f;
    public float minFresnelPower = 0.5f;

    private int currentStepIndex = 0;
    private string fresnelProperty = "_FresnelPower";

    public FixedController fixedController;
    public SilantroLever lever;

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
        Debug.Log("current step index : " + currentStepIndex);
        if (currentStepIndex < tutorialSequence.Length)
        {
            SoundName audioToPlay = tutorialSequence[currentStepIndex];
            Debug.Log("SoundName : " + audioToPlay);

            // Play the sound for this step 
            AudioManager.inst.PlaySound(audioToPlay);

            // Highlight the corresponding object, if available 
            if (tutorialObjects != null && currentStepIndex < tutorialObjects.Length)
            {
                GameObject obj = tutorialObjects[currentStepIndex];
                if (obj != null)
                {
                    HighlightObject(obj);
                }
            }
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


    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            StartCoroutine(AnimateFresnel(material));
        }
    }


    IEnumerator AnimateFresnel(Material material)
    {
        float initialFresnelPower = material.GetFloat(fresnelProperty);

        while (true)
        {
            float elapsedTime = 0f;

            // Increase Fresnel power 
            while (elapsedTime < highlightDuration)
            {
                elapsedTime += Time.deltaTime;
                float fresnelPower = Mathf.Lerp(minFresnelPower, maxFresnelPower, elapsedTime / highlightDuration);
                material.SetFloat(fresnelProperty, fresnelPower);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            elapsedTime = 0f;

            // Decrease Fresnel power back to normal
            while (elapsedTime < highlightDuration)
            {
                elapsedTime += Time.deltaTime;
                float fresnelPower = Mathf.Lerp(maxFresnelPower, minFresnelPower, elapsedTime / highlightDuration);
                material.SetFloat(fresnelProperty, fresnelPower);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            if (!AudioManager.inst.IsAudioPlaying())
            {
                material.SetFloat(fresnelProperty, initialFresnelPower);
                yield break;
            }
        }
    }

    public void recieveInput()
    {
        if (fixedController.isEngineTurnOn == true)
        {
            //play throttle clip
        }

        if (lever.isleverActive == true)
        {
            //play cyclic throttle clip
        }

        if (lever.isCyclicActive == true)
        {
            //play takeOff clip
        }


    }
}*/

using System.Collections;
using Oyedoyin.Common;
using Oyedoyin.FixedWing;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager inst;
    public SoundName[] tutorialSequence;
    public GameObject[] tutorialObjects;
    public float highlightDuration = 2f;
    public float maxFresnelPower = 5f;
    public float minFresnelPower = 0.5f;

    private int currentStepIndex = 0;
    private string fresnelProperty = "_FresnelPower";

    public FixedController fixedController;
    public SilantroLever lever;
    public SilantroLever cycliclever;

    private bool isTaskCompleted = false; 

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
        Debug.Log("Current step index : " + currentStepIndex);
        if (currentStepIndex < tutorialSequence.Length)
        {
            SoundName audioToPlay = tutorialSequence[currentStepIndex];
            Debug.Log("SoundName : " + audioToPlay);

            // Play the sound for this step 
            AudioManager.inst.PlaySound(audioToPlay);

            // Highlight the corresponding object, if available 
            if (tutorialObjects != null && currentStepIndex < tutorialObjects.Length)
            {
                GameObject obj = tutorialObjects[currentStepIndex];
                if (obj != null)
                {
                    HighlightObject(obj);
                }
            }

            if (currentStepIndex < 2)
            {
                // First two steps: Play without checking conditions
                StartCoroutine(WaitForAudioToFinish(false));
            }
            else
            {
                // For subsequent steps, check player input
                StartCoroutine(WaitForAudioToFinish(true));
            }
        }
    }

    IEnumerator WaitForAudioToFinish(bool checkTaskCompletion)
    {
        while (AudioManager.inst.IsAudioPlaying())
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        if (checkTaskCompletion)
        {
            StartCoroutine(CheckTaskCompletion());
        }
        else
        {
            ProceedToNextStep();
        }
    }

    IEnumerator CheckTaskCompletion()
    {
        isTaskCompleted = CheckCurrentInstructionCondition();

        while (!isTaskCompleted)
        {
            Debug.Log("Task not completed. Waiting 5 seconds to repeat the instruction.");
            yield return new WaitForSeconds(5f);

            // Repeat the same instruction
            AudioManager.inst.PlaySound(tutorialSequence[currentStepIndex]);
            while (AudioManager.inst.IsAudioPlaying())
            {
                yield return null;
            }

            // Recheck task condition after repeating the instruction
            isTaskCompleted = CheckCurrentInstructionCondition();
        }

        // If task is completed, proceed to the next step
        ProceedToNextStep();
    }

    void ProceedToNextStep()
    {
        Debug.Log("ProceedToNextStep called!");
        currentStepIndex++;
        if (currentStepIndex < tutorialSequence.Length)
        {
            PlayNextTutorialStep();
        }
        else
        {
            StopAudio();
        }
    }

    bool CheckCurrentInstructionCondition()
    {
        // Check conditions based on the current step
        if (currentStepIndex == 2)
        {
            return fixedController.isEngineTurnOn;
        }
        else if (currentStepIndex == 3)
        {
            return lever.isleverActive;
        }
        else if (currentStepIndex == 4)
        {
            return cycliclever.isCyclicActive;
        }
        return true; 
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


    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            StartCoroutine(AnimateFresnel(material));
        }
    }


    IEnumerator AnimateFresnel(Material material)
    {
        float initialFresnelPower = material.GetFloat(fresnelProperty);

        while (true)
        {
            float elapsedTime = 0f;

            // Increase Fresnel power 
            while (elapsedTime < highlightDuration)
            {
                elapsedTime += Time.deltaTime;
                float fresnelPower = Mathf.Lerp(minFresnelPower, maxFresnelPower, elapsedTime / highlightDuration);
                material.SetFloat(fresnelProperty, fresnelPower);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            elapsedTime = 0f;

            // Decrease Fresnel power back to normal
            while (elapsedTime < highlightDuration)
            {
                elapsedTime += Time.deltaTime;
                float fresnelPower = Mathf.Lerp(maxFresnelPower, minFresnelPower, elapsedTime / highlightDuration);
                material.SetFloat(fresnelProperty, fresnelPower);
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            if (!AudioManager.inst.IsAudioPlaying())
            {
                material.SetFloat(fresnelProperty, initialFresnelPower);
                yield break;
            }
        }
    }
}


