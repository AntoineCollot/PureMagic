using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onLevelCleared = new UnityEvent();
    public UnityEvent onLevelFailed = new UnityEvent();

    [HideInInspector]
    public static bool gameIsOver;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        gameIsOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void ClearLevel()
    {
        gameIsOver = true;
        onLevelCleared.Invoke();
    }

    public void GameOver()
    {
        gameIsOver = true;
        onLevelFailed.Invoke();
    }
}
