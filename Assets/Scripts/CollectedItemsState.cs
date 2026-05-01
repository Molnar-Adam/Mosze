using System;
using System.Collections.Generic;
using UnityEngine;

/// A játékos által összegyűjtött tárgyakat és kulcsokat globálisan nyilvántartó statikus osztály.
public static class CollectedItemsState
{
    private static int requiredItemCount = 3;

    /// Az eddig sikeresen felvett tárgyak ID-jának listája.
    private static readonly HashSet<string> CollectedItemIds = new HashSet<string>();

    /// A továbbjutáshoz szükséges tárgyak azonosítói.
    private static readonly HashSet<string> RequiredItemIds = new HashSet<string>()
    {
        "Map1_Key",
        "Map2_Key",
        "Map3_Key"
    };

    /// Inicializálja a szükséges tárgyak listáját a megadott konfiguráció (EventConfig) alapján.
    public static void InitializeFromConfig(string[] items)
    {
        if (items != null && items.Length > 0)
        {
            RequiredItemIds.Clear();
            foreach (var item in items)
            {
                RequiredItemIds.Add(item);
            }
            requiredItemCount = items.Length;
        }
    }

    /// Visszaadja, hogy összesen hány tárgyat kell összegyűjteni a továbbjutáshoz.
    public static int RequiredItemCount
    {
        get
        {
            return requiredItemCount;
        }
    }

    /// Visszaadja, hogy a játékos eddig hány megismételhetetlen (egyedi) tárgyat vett fel.
    public static int CollectedCount
    {
        get
        {
            return CollectedItemIds.Count;
        }
    }

    /// Megvizsgálja, hogy a játékos megszerezte-e az összes kötelező tárgyat.
    public static bool HasAllRequiredItems
    {
        get
        {
            return RequiredItemIds.IsSubsetOf(CollectedItemIds);
        }
    }


    /// Megpróbál felvenni egy tárgyat a megadott azonosítóval.
    public static bool TryCollect(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return false;
        }

        return CollectedItemIds.Add(itemId);
    }

    /// Ellenőrzi, hogy egy adott tárgyat felvett-e már a játékos.
    public static bool IsCollected(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return false;
        }

        return CollectedItemIds.Contains(itemId);
    }

    /// Kitörli az összes megszerzett tárgyat.
    public static void ResetProgress()
    {
        CollectedItemIds.Clear();
    }

    /// A Unity betöltésekor automatikusan lefutó metódus, ami üríti a memóriát az újrakezdéshez.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        CollectedItemIds.Clear();
    }
}