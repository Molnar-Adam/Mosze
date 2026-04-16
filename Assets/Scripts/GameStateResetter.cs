using UnityEngine;

public static class GameStateResetter
{
    public static void ResetGameState()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        PlayerHealth.ResetSavedHealth();
        CollectedItemsState.ResetProgress();
        LeverPuzzleSwitch.ResetPuzzleState();
        PianoPuzzle.ResetPuzzleState();

        Time.timeScale = 1f;
    }
}
