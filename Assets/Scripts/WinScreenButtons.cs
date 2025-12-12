using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButtons : MonoBehaviour
{
    public void PlayAgainButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitToMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
