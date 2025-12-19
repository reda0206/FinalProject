using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pauseMenuPrefab;

    private GameObject pauseMenuUi;
    public bool isPaused = false;
    private List<AudioSource> audioSources = new List<AudioSource>();
    public List<AudioSource> excludedAudioSources = new List<AudioSource>();

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
                pauseMenuUi = GameObject.Find("PauseMenu") ?? GameObject.Find("PauseMenuUi");

                if (pauseMenuUi == null && pauseMenuPrefab != null)
                {
                    Canvas canvas = FindObjectOfType<Canvas>();
                    pauseMenuUi = canvas != null ? Instantiate(pauseMenuPrefab, canvas.transform) : Instantiate(pauseMenuPrefab);
                    pauseMenuUi.name = pauseMenuPrefab.name;
                }

                if (pauseMenuUi != null)
                {
                    pauseMenuUi.SetActive(false);
                    AssignPauseMenuButtons();
                }
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUi.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        var camFollow = Camera.main?.GetComponent<CameraFollowPlayer>();
        if (camFollow != null)
            camFollow.enabled = false;
        PauseAudio();
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenuUi.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        var camFollow = Camera.main?.GetComponent<CameraFollowPlayer>();
        if (camFollow != null)
            camFollow.enabled = true;

        ResumeAudio();
        Time.timeScale = 1f;
    }

    public void PauseAudio()
    {
        foreach (AudioSource audio in FindObjectsOfType<AudioSource>())
        {
            if (!excludedAudioSources.Contains(audio) && audio.isPlaying)
            {
                audio.Pause();
                audioSources.Add(audio);
            }
        }
    }

    public void ResumeAudio()
    {
        for (int i = audioSources.Count - 1; i >= 0; i--)
        {
            if (audioSources[i])
            {
                audioSources[i].UnPause();
                audioSources.RemoveAt(i);
            }
        }
    }

    public void QuitToMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }
}
