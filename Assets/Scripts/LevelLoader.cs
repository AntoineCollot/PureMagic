using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    bool isLoading = false;

    public void LoadMenu()
    {
        LoadLevel(0);
    }

    public void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int id)
    {
        if (isLoading)
            return;

        isLoading = true;
        SceneManager.LoadScene(0);
    }
}
