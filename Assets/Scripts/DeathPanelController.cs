using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathPanelController : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private PlayerHealth playerHealth;

    public static bool IsDeathScreenActive { get; private set; }

    private bool deathShown;

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

    public void Retry()
    {
        GameStateResetter.ResetGameState();
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {

    }

}
