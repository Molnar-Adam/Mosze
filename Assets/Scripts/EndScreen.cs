using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class EndScreen : MonoBehaviour
{
   
    private void Awake()
    {
        EnsureEventSystem();
    }

    /// Visszatér a főmenübe.
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// Bezárja az alkalmazást.
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
