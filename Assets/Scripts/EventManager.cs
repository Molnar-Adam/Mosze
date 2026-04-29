using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GameEventsConfig
{
    public List<DialogueData> dialogues = new List<DialogueData>();
    public List<PuzzleData> puzzles = new List<PuzzleData>();
    public List<LeverPuzzleGroupData> leverPuzzles = new List<LeverPuzzleGroupData>();
    public string[] requiredItemIds = new string[] { "Map1_Key", "Map2_Key", "Map3_Key" };
}

[System.Serializable]
public class DialogueData
{
    public string dialogueID;
    public string[] lines;
}

[System.Serializable]
public class PuzzleData
{
    public string puzzleID;
    public int[] requiredPatternIndices;
}

[System.Serializable]
public class LeverPuzzleGroupData
{
    public string puzzleID; // Pl. "LeverPuzzle_Scene1"
    public int requiredLeverCount = 5;
    public List<LeverData> levers = new List<LeverData>();
}

[System.Serializable]
public class LeverData
{
    public int leverIndex; // 1-től 5-ig (vagy ameddig tart)
    public int[] affectedLevers;
}

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    public GameEventsConfig EventConfig;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, "gameEvents.json");
            LoadEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveEvents()
    {
        string json = JsonUtility.ToJson(EventConfig, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Events saved to: " + saveFilePath);
    }

    public void LoadEvents()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            EventConfig = JsonUtility.FromJson<GameEventsConfig>(json);
        }
        else
        {
            EventConfig = new GameEventsConfig();
            
            // Templates
            EventConfig.dialogues.Add(new DialogueData { dialogueID = "NPC_1", lines = new string[] { "Szia!", "Kezdd el a kalandot." } });
            EventConfig.puzzles.Add(new PuzzleData { puzzleID = "Piano_1", requiredPatternIndices = new int[] { 0, 1, 2 } });
            
            var sampleLeverGroup = new LeverPuzzleGroupData { puzzleID = "LeverPuzzle_Scene1", requiredLeverCount = 5 };
            sampleLeverGroup.levers.Add(new LeverData { leverIndex = 1, affectedLevers = new int[] { 2, 3 } });
            sampleLeverGroup.levers.Add(new LeverData { leverIndex = 2, affectedLevers = new int[] { 1, 4 } });
            EventConfig.leverPuzzles.Add(sampleLeverGroup);

            SaveEvents();
        }

        // Initialize static states that depend on these configs
        CollectedItemsState.InitializeFromConfig(EventConfig.requiredItemIds);
    }

    public string[] GetDialogueLines(string dialogueID)
    {
        var dialogue = EventConfig.dialogues.FirstOrDefault(d => d.dialogueID == dialogueID);
        return dialogue?.lines;
    }

    public int[] GetPuzzlePattern(string puzzleID)
    {
        var puzzle = EventConfig.puzzles.FirstOrDefault(p => p.puzzleID == puzzleID);
        return puzzle?.requiredPatternIndices;
    }

    public int[] GetAffectedLevers(string puzzleID, int leverIndex)
    {
        var puzzleGroup = EventConfig.leverPuzzles.FirstOrDefault(l => l.puzzleID == puzzleID);
        if (puzzleGroup != null)
        {
            var leverData = puzzleGroup.levers.FirstOrDefault(l => l.leverIndex == leverIndex);
            if (leverData != null)
            {
                return leverData.affectedLevers;
            }
        }
        return null;
    }

    public int GetRequiredLeverCount(string puzzleID)
    {
        var puzzleGroup = EventConfig.leverPuzzles.FirstOrDefault(l => l.puzzleID == puzzleID);
        return puzzleGroup != null ? puzzleGroup.requiredLeverCount : 5;
    }
}