using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Level2");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
