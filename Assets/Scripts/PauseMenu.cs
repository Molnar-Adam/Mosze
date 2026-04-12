using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    private void Awake()
    {
        EnsureEventSystem();
    }

    void Update()
    {
        if (DeathPanelController.IsDeathScreenActive)
        {
            if (PausePanel != null && PausePanel.activeSelf)
            {
                PausePanel.SetActive(false);
            }

            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel == null)
            {
                return;
            }

            if (Time.timeScale == 0)
            {
                Resume();
            }
            else
            {
                PausePanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void Resume()
    {
            if (DeathPanelController.IsDeathScreenActive)
            {
                return;
            }

            if (PausePanel == null)
            {
                return;
            }

            PausePanel.SetActive(false);
            Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {

    }

    private void EnsureEventSystem()
    {
        if (EventSystem.current != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }
}
