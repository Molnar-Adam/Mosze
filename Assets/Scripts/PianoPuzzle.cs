using UnityEngine;
using UnityEngine.UI;

/// A zongora játékelem logikáját vezérlő osztály (kottasorozat ellenőrzése és teljesítés).
public class PianoPuzzle : MonoBehaviour
{
    /// <summary>Egyedi azonosító a zongora rejtvényhez, amely alapján a mintát lekérdezi.</summary>
    [SerializeField] private string puzzleID;
    
    /// A zongora gombjait tároló tömb.
    [SerializeField] private Button[] pianoButtons;
    
    private Button[] requiredPattern;
    
    /// A játékobjektum, ami eltűnik a rejtvény megoldásakor.
    [SerializeField] private GameObject objectToDestroy;
    
    /// Hivatkozás a PianoInteract szkriptre az interakciók zárolása miatt.
    [SerializeField] private PianoInteract pianoInteract;

    private int currentIndex;
    private bool solved;
    private bool targetHiddenByPuzzle;

    /// Számon tartja, hogy a rejtvény meg lett-e oldva.
    public bool IsSolved
    {
        get
        {
            return solved;
        }
    }

    /// Játék indulásakor lekéri az EventManagertől a megoldási mintát és feltölti a tömböt.
    private void Start()
    {
        if (EventManager.Instance != null && !string.IsNullOrEmpty(puzzleID))
        {
            int[] loadedPattern = EventManager.Instance.GetPuzzlePattern(puzzleID);
            if (loadedPattern != null && loadedPattern.Length > 0 && pianoButtons != null)
            {
                requiredPattern = new Button[loadedPattern.Length];
                for (int i = 0; i < loadedPattern.Length; i++)
                {
                    if(loadedPattern[i] >= 0 && loadedPattern[i] < pianoButtons.Length)
                        requiredPattern[i] = pianoButtons[loadedPattern[i]];
                }
            }
        }
    }

    /// Ha nincs beállítva a pianoInteract, megpróbálja megkeresni a komponensek között, majd feliratkozik a gombokra.
    private void Awake()
    {
        if (pianoInteract == null)
        {
            pianoInteract = GetComponent<PianoInteract>();
        }

        RegisterButtonListeners();
    }

    /// Kezeli egy gomb lenyomását a zongorán, és ellenőrzi a minta helyességét.
    public void PressKey(Button pressedButton)
    {
        if (solved)
        {
            return;
        }

        if (pressedButton == null || requiredPattern == null || requiredPattern.Length == 0)
        {
            return;
        }

        if (pressedButton == requiredPattern[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= requiredPattern.Length)
            {
                Solve();
            }

            return;
        }

        currentIndex = pressedButton == requiredPattern[0] ? 1 : 0;
    }

    /// Globálisan minden PianoPuzzle objektumon meghívja az alaphelyzetbe állítás metódusát.
    public static void ResetPuzzleState()
    {
        var puzzles = Object.FindObjectsByType<PianoPuzzle>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var puzzle in puzzles)
    /// Alaphelyzetbe állítja az adott zongora puzzle állapotát.
        {
            puzzle.ResetPuzzleInstance();
        }
    }

    private void ResetPuzzleInstance()
    {
        solved = false;
        currentIndex = 0;

        if (targetHiddenByPuzzle && objectToDestroy != null)
        {
            objectToDestroy.SetActive(true);
        }

    /// Sikeres megoldás esetén zárolja a zongorát és eltünteti az akadályt.
        targetHiddenByPuzzle = false;

        if (pianoInteract != null)
        {
            pianoInteract.ResetInteractionState();
        }
    }

    private void Solve()
    {
        solved = true;

        if (pianoInteract != null)
        {
            pianoInteract.LockInteraction();
        }
    /// Beállítja a gombokhoz tartozó eseményfigyelőket a kattintásokhoz.
    
        if (objectToDestroy != null)
        {
            objectToDestroy.SetActive(false);
            targetHiddenByPuzzle = true;
        }
    }

    private void RegisterButtonListeners()
    {
        if (pianoButtons == null)
        {
            return;
        }

        foreach (Button button in pianoButtons)
        {
            if (button == null)
            {
                continue;
            }

            Button capturedButton = button;
            button.onClick.AddListener(() => PressKey(capturedButton));
        }
    }

}
