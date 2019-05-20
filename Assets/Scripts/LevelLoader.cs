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

    public void LoadTutoLevel()
    {
        LoadLevel(1);
    }

    public void LoadRealLevel()
    {
        LoadLevel(8);
    }

    public void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int id)
    {
        if (isLoading)
            return;

        isLoading = true;
        SceneManager.LoadScene(id);
    }
}
