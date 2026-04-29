using UnityEngine;
using UnityEngine.UI;

public class PianoPuzzle : MonoBehaviour
{
    [SerializeField] private string puzzleID;
    [SerializeField] private Button[] pianoButtons;
    private Button[] requiredPattern;
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private PianoInteract pianoInteract;

    private int currentIndex;
    private bool solved;
    private bool targetHiddenByPuzzle;

    public bool IsSolved
    {
        get
        {
            return solved;
        }
    }

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

    private void Awake()
    {
        if (pianoInteract == null)
        {
            pianoInteract = GetComponent<PianoInteract>();
        }

        RegisterButtonListeners();
    }

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

    public void ResetProgress()
    {
        currentIndex = 0;
    }

    public static void ResetPuzzleState()
    {
        var puzzles = Object.FindObjectsByType<PianoPuzzle>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var puzzle in puzzles)
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
