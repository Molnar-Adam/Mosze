using UnityEngine;
using UnityEngine.SceneManagement;

/// A játékos halálakor megjelenő UI panel vezérléséért felelős osztály.
/// Megállítja az időt és megjeleníti az úrjakezdési, vagy kilépési opciókat.
public class DeathPanelController : MonoBehaviour
{
    /// A halálképernyőt tartalmazó, általában kikapcsolt GameObject UI panel.
    [SerializeField] private GameObject deathPanel;

    /// A játékos életerejét kezelő komponens. Erre iratkozik fel, hogy figyelje a halált.
    [SerializeField] private PlayerHealth playerHealth;

    /// Globális jelző, ami mutatja, hogy jelenleg aktív-e a halálképernyő.
    public static bool IsDeathScreenActive { get; private set; }

    /// Belső változó, megakadályozza a halálpanel többszöri meghívását egyszerre.
    private bool deathShown;

    /// Unity betöltésekor automatikusan alaphelyzetbe állítja a halálképernyő státuszát.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        IsDeathScreenActive = false;
    }

    private void Awake()
    {
        IsDeathScreenActive = false;

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        TrySubscribeToPlayer();
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied -= ShowDeathPanel;
        }
    }

    private void Update()
    {
        if (!deathShown && playerHealth == null)
        {
            TrySubscribeToPlayer();
        }
    }

    /// Megpróbálja megkeresni a játékos karaktert, és feliratkozni az OnDied eseményére.
    private void TrySubscribeToPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied -= ShowDeathPanel;
            playerHealth.OnDied += ShowDeathPanel;
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            return;
        }

        playerHealth = playerObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDied -= ShowDeathPanel;
            playerHealth.OnDied += ShowDeathPanel;
        }
    }

    /// Aktiválja a halálpanelt és megállítja a játékidőt.
    private void ShowDeathPanel()
    {
        if (deathShown)
        {
            return;
        }

        deathShown = true;
        IsDeathScreenActive = true;

        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    /// Újraindítja az első pályát, és alaphelyzetbe állítja a mentett állásokat.
    public void Retry()
    {
        GameStateResetter.ResetGameState();
        SceneManager.LoadScene("Map 1"); 
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
}
