using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollectedItemsState
{
    public const int RequiredItemCount = 3;

    private static readonly HashSet<string> CollectedItemIds = new HashSet<string>();
    private static readonly HashSet<string> RequiredItemIds = new HashSet<string>()
    {
        "Map1_Key",
        "Map2_Key",
        "Map3_Key"
    };

    public static int CollectedCount
    {
        get
        {
            return CollectedItemIds.Count;
        }
    }

    public static bool HasAllRequiredItems
    {
        get
        {
            return RequiredItemIds.IsSubsetOf(CollectedItemIds);
        }
    }

    public static bool TryCollect(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return false;
        }

        return CollectedItemIds.Add(itemId);
    }

    public static bool IsCollected(string itemId)
    {
        if (string.IsNullOrWhiteSpace(itemId))
        {
            return false;
        }

        return CollectedItemIds.Contains(itemId);
    }

    public static void ResetProgress()
    {
        CollectedItemIds.Clear();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeState()
    {
        CollectedItemIds.Clear();
    }
}