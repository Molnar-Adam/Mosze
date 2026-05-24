using UnityEngine;
using System.Collections.Generic;

/// A játék globális, memóriában tárolt állapotait nyilvántartó statikus osztály.
public static class GameState
{
    /// Jelzi, hogy a játékban be van-e kapcsolva az áramforrás.
    public static bool powerOn = false;

    /// Egy másik scene-ben lévő, megsemmisítésre ítélt objektumok azonosítói.
    public static HashSet<string> destroyedObjects = new HashSet<string>();
}
