using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// A Pause működését (megjelenítés, eltüntetés, újraindulás) kezelő szkript.
public class PauseMenu : MonoBehaviour
{
    /// A menüt tartalmazó UI Panel gameObject.
    public GameObject PausePanel;

    /// Induláskor biztosítja az EventSystem meglétét, hogy a UI gombok működjenek.
    private void Awake()
    {
        EnsureEventSystem();
    }

    /// Billentyűzetes interakciókat (Escape) figyel, valamint más menük/képernyők zavarását blokkolja.
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

        if (PianoInteract.IsAnyPianoUIOpen)
        {
            return;
        }

        if (PianoInteract.LastPianoClosedWithEscapeFrame == Time.frameCount)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausePanel == null)
            {
                return;
            }

            if (PausePanel.activeSelf)
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

    /// Eltünteti a panelt, és újraindítja a játékidőt (Time.timeScale = 1).
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

    /// "Menu" scene-re váltás
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    /// Kilépés a játékból
    public void Quit()
    {
        Application.Quit();
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
