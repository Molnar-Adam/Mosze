using UnityEngine;
using UnityEngine.SceneManagement;

/// A főmenü működését (játék indítása, kilépés) szabályozó osztály.
public class MainMenu : MonoBehaviour
{
        /// Törli az eddigi játékállapotot és betölti a következő jelenetet (első pályát).
        public void PlayGame()
        {
                GameStateResetter.ResetGameState();
                SceneManager.LoadScene("Map 1");
        }

        /// Kilép a játékból.
        public void QuitGame()
        {
                Application.Quit();
        }
}
