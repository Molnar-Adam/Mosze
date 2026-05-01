using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// Egy olyan kart kezelő script, amely része egy több karból álló rejtvénynek.
public class LeverPuzzleSwitch : MonoBehaviour
{
    [SerializeField] private string puzzleID; 
    [SerializeField] [Range(1, 5)] private int leverIndex; 
    private int requiredLeverCount = 5;

    /// Azoknak a karoknak az indexei, amelyeket ez a kapcsoló maga is átvált mehhúzáskor.
    private List<int> affectedLevers = new List<int>();

    /// A kar SpriteRenderer-e.
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    /// Felfelé állapot sprite-ja.
    [SerializeField] private Sprite upSprite;
    
    /// Lefelé állapot sprite-ja.
    [SerializeField] private Sprite downSprite;

    /// Felolvassa az EventManagerből, hogy hány kar alkotja a feladványt, 
    /// és melyik másik karokra van hatással ennek a karnak a meghúzása.
    private void Start()
    {
        if (EventManager.Instance != null && !string.IsNullOrEmpty(puzzleID))
        {
            requiredLeverCount = EventManager.Instance.GetRequiredLeverCount(puzzleID);
            
            var affectedData = EventManager.Instance.GetAffectedLevers(puzzleID, leverIndex);
            if (affectedData != null)
            {
                affectedLevers = new List<int>(affectedData);
            }
        }
    }

     private KeyCode interactKey = KeyCode.E;
     private string playerTag = "Player";
     
    /// Az interakciót jelző felirat.
    [SerializeField] private TextMeshProUGUI interactText;

    /// Az ajtó, amit ez a rejtvény kinyit/eltüntet megfejtéskor.
    [SerializeField] private GameObject doorToOpen;

    private bool canInteract;

    private static PuzzleRuntimeData runtimeData = new PuzzleRuntimeData();

    /// Alapállapotba állítja a karos puzzle memóriabeli adatait, és minden kart alaphelyzetbe kapcsol.
    public static void ResetPuzzleState()
    {
        runtimeData = new PuzzleRuntimeData();

        var levers = Object.FindObjectsByType<LeverPuzzleSwitch>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var lever in levers)
        {
            lever.RegisterLever();
            lever.canInteract = false;

            if (lever.interactText != null)
            {
                lever.interactText.gameObject.SetActive(false);
            }

            lever.RefreshVisualFromState();
        }
    }

    /// A karos rejtvények futásidejű, memóriában tárolt állapota.
    private class PuzzleRuntimeData
    {
        /// A karok aktuális állapota indexük szerint.
        public readonly Dictionary<int, bool> states = new Dictionary<int, bool>();
        
        /// A játékban lévő összes érintett kar szkriptje.
        public readonly List<LeverPuzzleSwitch> levers = new List<LeverPuzzleSwitch>();
        
        /// Igaz, ha a feladvány már meg lett oldva.
        public bool solved;
    }

    /// Editorban segít lekérni az alapvető SpriteRenderer komponenst.
    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// Lekéri a grafikus megjelenítőt, illetve kikapcsolja a felvétel szöveget az indulás pillanatában.
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    /// Objektum bekapcsolásakor regisztrálja magát a globális rejtvény adatba és frissíti a saját grafikáját.
    private void OnEnable()
    {
        RegisterLever();
        RefreshVisualFromState();
    }

    /// Kikapcsoláskor kiregisztrálja magát, letiltja az E-betűs feliratot, és kiveszi az interakciót.
    private void OnDisable()
    {
        UnregisterLever();

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }

        canInteract = false;
    }

    private void Update()
    {
        if (canInteract && Input.GetKeyDown(interactKey))
        {
            PullLever();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
    /// Ha a játékos elhagyja a zónát, letiltja az állítást és eltünteti a feliratot.
        {
            return;
        }

        canInteract = true;

        if (interactText != null)
        {
            interactText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
        {
            return;
        }
    /// Amikor a játékos meghúzza a kart: Átváltja ezen kar és az összes hozzá csatlakozó kar "states" állapotát.
    /// Utána frissíti a grafikát, és ellenőrzi a rejtvény megoldását.
    
        canInteract = false;

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void PullLever()
    {
        var data = GetData();
        var targets = GetEffectiveTargets();

        foreach (var target in targets)
        {
            var current = GetLeverState(data, target);
            SetLeverState(data, target, !current);
        }

        RefreshAllLeverVisuals(data);
        EvaluateSolved(data);
    }

    /// Felveszi a jelenlegi kart az Event rendszerébe az aktuális Dictionary adattal együtt.
    private void RegisterLever()
    {
        var data = GetData();

        if (!data.levers.Contains(this))
        {
            data.levers.Add(this);
        }

        EnsureStateExists(data, leverIndex);
    }

    /// Törli magát a konfigurációból, és nullázza az egész RuntimeData-t, ha ez volt az utolsó kar.
    private void UnregisterLever()
    {
        var data = GetData();

        data.levers.Remove(this);

        if (data.levers.Count == 0)
        {
            runtimeData = new PuzzleRuntimeData();
        }
    }

    private PuzzleRuntimeData GetData()
    {
        return runtimeData;
    }

    private List<int> GetEffectiveTargets()
    {
        if (affectedLevers == null || affectedLevers.Count == 0)
        {
            return new List<int>();
        }

        var distinct = new HashSet<int>();

        foreach (var lever in affectedLevers)
        {
            if (lever >= 1 && lever <= 5)
            {
                distinct.Add(lever);
            }
        }

        return new List<int>(distinct);
    }

    /// Lecseréli a karhoz tartozó képet.
    private void RefreshVisualFromState()
    {
        var data = GetData();
        EnsureStateExists(data, leverIndex);
        ApplyLeverSprite(GetLeverState(data, leverIndex));
    }

    private void RefreshAllLeverVisuals(PuzzleRuntimeData data)
    {
        foreach (var lever in data.levers)
        {
            lever.ApplyLeverSprite(GetLeverState(data, lever.leverIndex));
        }
    }

    private void ApplyLeverSprite(bool isDown)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (isDown && downSprite != null)
        {
            spriteRenderer.sprite = downSprite;
        }
        else if (!isDown && upSprite != null)
        {
            spriteRenderer.sprite = upSprite;
        }
    }

    /// Statikus metódus. Biztosítja, hogy egy kar adatszerkezete legalább false értékkel benne legyen a Dictionaryben, ha eddig hiányzott.
    private static void EnsureStateExists(PuzzleRuntimeData data, int index)
    {
        if (!data.states.ContainsKey(index))
        {
            data.states[index] = false;
        }
    }

    /// Visszaadja a megadott sorszámú kar jelenlegi fizikai pozíciójának logikai étékét.
    private static bool GetLeverState(PuzzleRuntimeData data, int index)
    {
        EnsureStateExists(data, index);
    /// Megváltoztatja a megadott kar állapotát a RuntimeData dictionary-ban.
        return data.states[index];
    }

    private static void SetLeverState(PuzzleRuntimeData data, int index, bool value)
    {
        EnsureStateExists(data, index);
    /// Megvizsgálja, hogy minden kar lenn van-e. Ha igen, mindannyiukon keresztül kinyitja az ajtót.
        data.states[index] = value;
    }

    private void EvaluateSolved(PuzzleRuntimeData data)
    {
        var solvedNow = true;

        for (var i = 1; i <= requiredLeverCount; i++)
        {
            if (!GetLeverState(data, i))
            {
                solvedNow = false;
                break;
            }
        }

        if (!solvedNow || data.solved)
        {
            return;
        }

        bool belongsToPuzzle = !string.IsNullOrEmpty(puzzleID);
        foreach (var lever in data.levers)
        {
            if (!belongsToPuzzle || lever.puzzleID == this.puzzleID)
            {
                lever.OpenDoorAndInvokeSolvedEvent();
    /// Megsemmisíti az ajtó GameObjectet, ami be lett neki állítva az Editorban.
            }
        }
        
        data.solved = true;
    }

    private void OpenDoorAndInvokeSolvedEvent()
    {
        if (doorToOpen != null)
        {
            Destroy(doorToOpen);
        }
    }
}
