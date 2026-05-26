using UnityEngine;
using UnityEngine.SceneManagement;

// Ez a script a dialogue.cs és a scene swaphoz szükséges scripteket kombinálja
public class DialogueSceneTransition : MonoBehaviour
{
    //Dialógus komponens a dialogue script futtatásához
    [SerializeField] private Dialogue targetDialogue;

    //sceneswaphoz szükséges változók
    [SerializeField] private string targetSceneName;

    [SerializeField] private string targetSpawnId;

    [SerializeField] private string playerTag = "Player";

    [SerializeField] private string targetCameraName;

    // id hogy a destroyolandó object a scene újboli betöltése után is destroyed maradjon
    [SerializeField] private string destroyStateIdentifier;


    // Scene betöltésekor megnézi, hogy destroyednak kell-e lennie az objectnek és ha igen destroyolja
    private void Start()
    {
        if (!string.IsNullOrEmpty(destroyStateIdentifier) && GameState.destroyedObjects.Contains(destroyStateIdentifier))
        {
            Destroy(gameObject);
        }
    }

    // Feliratkozik a dialógus befejezését jelző eseményre, ha a komponens létezik
    private void OnEnable()
    {
        if (targetDialogue != null)
        {
            targetDialogue.OnDialogueFinished += HandleDialogueFinished;
        }
    }

    // Leiratkozik az eseményről, ha az objektum kikapcsol / megsemmisül, hogy elkerüljük a memóriaszivárgást
    private void OnDisable()
    {
        if (targetDialogue != null)
        {
            targetDialogue.OnDialogueFinished -= HandleDialogueFinished;
        }
    }

    // Ez a metódus hívódik meg, amikor a dialógus véget ér.
    private void HandleDialogueFinished()
    {
        // Ellenőrzi, hogy be van-e állítva, melyik jelenetet kell betölteni
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("A cél jelenet neve (targetSceneName) nincs beállítva a DialogueSceneTransition scriptben!");
            return;
        }

        // Ha van megadva egyedi azonosító, akkor rögzíti a GameState-ben, hogy véglegesen meg lett semmisítve
        if (!string.IsNullOrEmpty(destroyStateIdentifier))
        {
            GameState.destroyedObjects.Add(destroyStateIdentifier);
        }

        Destroy(gameObject);

        SceneSpawnSystem.SetNextSpawn(targetSpawnId, playerTag, targetCameraName);

        SceneManager.LoadScene(targetSceneName);
    }
}
