using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject pauseMenuUi;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" || scene.name == "Level2")
        {
            {
            GameObject found = null;
            try
            {
                found = GameObject.Find("PauseMenu");
            }
            catch
            {

            }

            if (found = null)
            {
                found = GameObject.Find("PauseMenuUi") ?? GameObject.Find("PauseMenu");
            }
            pauseMenuUi = found;

            if (pauseMenuUi != null)
            {
                pauseMenuUi.SetActive(false);

                AssignPauseMenuButtons();
            }
        }
    }

    private void AssignPauseMenuButtons()
    {
        if (pauseMenuUi == null) return;

        Button resumeButton = pauseMenuUi.transform.Find("ResumeButton")?.GetComponent<Button>();
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(Resume);
        }

        Button quitToMenuButton = pauseMenuUi.transform.Find("QuitToMenuButton")?.GetComponent<Button>();
        if (quitToMenuButton != null)
        {
            quitToMenuButton.onClick.RemoveAllListeners();
            quitToMenuButton.onClick.AddListener(QuitToMenuButton);
        }

        Button quitGameButton = pauseMenuUi.transform.Find("QuitGameButton")?.GetComponent<Button>();
        {
            quitGameButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.AddListener(QuitGameButton);
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
