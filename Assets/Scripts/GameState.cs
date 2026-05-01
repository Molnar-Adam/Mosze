using UnityEngine;

/// A játék globális, memóriában tárolt állapotait nyilvántartó statikus osztály.
public static class GameState
{
    /// Jelzi, hogy a játékban be van-e kapcsolva a fő áramforrás (pl. villanykapcsoló).
    public static bool powerOn = false;
}
