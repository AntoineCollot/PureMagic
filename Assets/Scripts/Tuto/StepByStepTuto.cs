using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StepByStepTuto : MonoBehaviour
{
    public TutoStep[] tutoSteps;
    int currentStep = -1;

    // Start is called before the first frame update
    void Start()
    {
        NextStep();
    }

    void NextStep()
    {
        currentStep++;

        if (currentStep >= tutoSteps.Length || !gameObject.activeInHierarchy)
            return;

        tutoSteps[currentStep].onStepStart.Invoke();

        if (tutoSteps[currentStep].useTime)
            Invoke("NextStep", tutoSteps[currentStep].timeLength);
    }

    [System.Serializable]
    public class TutoStep
    {
        public string name;
        public bool useTime;
        public float timeLength;
        public UnityEvent onStepStart = new UnityEvent();
    }
}
