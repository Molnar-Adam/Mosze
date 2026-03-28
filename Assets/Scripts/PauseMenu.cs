using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private string mainMenuSceneName = "Menu";

    public static bool IsPaused { get; private set; }

    private void Start()
    {
        Resume();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        Resume();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Resume();
        Application.Quit();
    }

    private void OnDisable()
    {
        if (IsPaused)
        {
            Resume();
        }
    }
}
