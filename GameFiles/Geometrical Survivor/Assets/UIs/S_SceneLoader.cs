using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_SceneLoader : MonoBehaviour
{
    public void LoadScene(string p_sceneName)
    {
        SceneManager.LoadScene(p_sceneName);

        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}