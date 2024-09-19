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
            AudioManager.inst.PlaySound(audioToPlay);

            // Highlight object if present 
            if (tutorialObjects != null && currentStepIndex < tutorialObjects.Length)
            {

                GameObject obj = tutorialObjects[currentStepIndex];
                if (obj != null)
                {
                    HighlightObject(obj);
                }
            }


            bool checkTaskCompletion = currentStepIndex >= 1;
            
            StartCoroutine(WaitForAudioToFinish(checkTaskCompletion));
        }
    }

    IEnumerator WaitForAudioToFinish(bool checkTaskCompletion)
    {
        // Wait until audio finishes 
        while (AudioManager.inst.IsAudioPlaying())
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);

        if (checkTaskCompletion)
        {
            // Wait until completion 
            yield return StartCoroutine(CheckTaskCompletion());
        }
        else
        {
            ProceedToNextStep();
        }
    }

    IEnumerator CheckTaskCompletion()
    {
        isTaskCompleted = CheckCurrentInstructionCondition();

        // Get the current GameObject to highlight
        GameObject currentObject = null;
        if (tutorialObjects != null && currentStepIndex < tutorialObjects.Length)
        {
            currentObject = tutorialObjects[currentStepIndex];
        }

     
        if (currentObject != null)
        {
            HighlightObject(currentObject); 
        }

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

        // Move to the next step
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
        if (currentStepIndex == 1)
        {
            Debug.Log("EngineOn");
            return fixedController.isEngineTurnOn;
        }
        else if (currentStepIndex == 2)
        {
            Debug.Log("leverSet");
            return lever.isleverActive;
        }
        else if (currentStepIndex == 3)
        {
            Debug.Log("cyclicLever Set");
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

