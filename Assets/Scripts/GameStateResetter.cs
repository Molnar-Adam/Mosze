using UnityEngine;

/// A játékállapot teljes alaphelyzetbe állításáért felelős statikus osztály.
public static class GameStateResetter
{
    /// Törli a mentett beállításokat, visszaállítja az életerőt, a tárgyakat és rejtvényeket.
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
