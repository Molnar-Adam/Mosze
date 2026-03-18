using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollectedItemsState
{
    public const int RequiredItemCount = 3;

    private static readonly HashSet<string> CollectedItemIds = new HashSet<string>();

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
            return CollectedCount >= RequiredItemCount;
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