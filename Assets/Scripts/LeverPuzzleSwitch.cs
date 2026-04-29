using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverPuzzleSwitch : MonoBehaviour
{
    [SerializeField] private string puzzleID; // Pl.: "LeverPuzzle_Scene1"
    [SerializeField] [Range(1, 5)] private int leverIndex; // A kar saját száma: 1..5
    private int requiredLeverCount = 5;

    private List<int> affectedLevers = new List<int>();

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite upSprite;
    [SerializeField] private Sprite downSprite;

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
    [SerializeField] private TextMeshProUGUI interactText;

    [SerializeField] private GameObject doorToOpen;

    private bool canInteract;

    private static PuzzleRuntimeData runtimeData = new PuzzleRuntimeData();

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

    private class PuzzleRuntimeData
    {
        public readonly Dictionary<int, bool> states = new Dictionary<int, bool>();
        public readonly List<LeverPuzzleSwitch> levers = new List<LeverPuzzleSwitch>();
        public bool solved;
    }

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void OnEnable()
    {
        RegisterLever();
        RefreshVisualFromState();
    }

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

    private void RegisterLever()
    {
        var data = GetData();

        if (!data.levers.Contains(this))
        {
            data.levers.Add(this);
        }

        EnsureStateExists(data, leverIndex);
    }

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

    private static void EnsureStateExists(PuzzleRuntimeData data, int index)
    {
        if (!data.states.ContainsKey(index))
        {
            data.states[index] = false;
        }
    }

    private static bool GetLeverState(PuzzleRuntimeData data, int index)
    {
        EnsureStateExists(data, index);
        return data.states[index];
    }

    private static void SetLeverState(PuzzleRuntimeData data, int index, bool value)
    {
        EnsureStateExists(data, index);
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
