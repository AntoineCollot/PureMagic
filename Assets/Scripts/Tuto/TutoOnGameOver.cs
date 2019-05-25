using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoOnGameOver : MonoBehaviour
{
    public StepByStepTuto failedPath;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onLevelFailed.AddListener(OnTutoFailed);
    }

    public void OnTutoFailed()
    {
        failedPath.enabled = true;
    }
}
