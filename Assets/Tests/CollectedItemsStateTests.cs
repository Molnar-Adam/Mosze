using NUnit.Framework;

public class CollectedItemsStateTests
{
    [SetUp]
    public void Setup()
    {
        CollectedItemsState.ResetProgress();
    }

    [Test]
    public void TryCollect_AddsItem()
    {
        bool result = CollectedItemsState.TryCollect("Map1_Key");

        Assert.IsTrue(result);
        Assert.AreEqual(1, CollectedItemsState.CollectedCount);
    }

    [Test]
    public void TryCollect_DuplicateItem_ReturnsFalse()
    {
        CollectedItemsState.TryCollect("Map1_Key");
        bool result = CollectedItemsState.TryCollect("Map1_Key");

        Assert.IsFalse(result);
        Assert.AreEqual(1, CollectedItemsState.CollectedCount);
    }

    [Test]
    public void TryCollect_InvalidItem_ReturnsFalse()
    {
        Assert.IsFalse(CollectedItemsState.TryCollect(null));
        Assert.IsFalse(CollectedItemsState.TryCollect(""));
        Assert.IsFalse(CollectedItemsState.TryCollect("   "));
    }

    [Test]
    public void IsCollected_ReturnsTrue_WhenCollected()
    {
        CollectedItemsState.TryCollect("Map1_Key");

        Assert.IsTrue(CollectedItemsState.IsCollected("Map1_Key"));
    }

    [Test]
    public void IsCollected_ReturnsFalse_WhenNotCollected()
    {
        Assert.IsFalse(CollectedItemsState.IsCollected("Map1_Key"));
    }

    [Test]
    public void HasAllRequiredItems_ReturnsFalse_Initially()
    {
        Assert.IsFalse(CollectedItemsState.HasAllRequiredItems);
    }

    [Test]
    public void HasAllRequiredItems_ReturnsTrue_WhenAllCollected()
    {
        CollectedItemsState.TryCollect("Map1_Key");
        CollectedItemsState.TryCollect("Map2_Key");
        CollectedItemsState.TryCollect("Map3_Key");

        Assert.IsTrue(CollectedItemsState.HasAllRequiredItems);
    }

    [Test]
    public void HasAllRequiredItems_IgnoresExtraItems()
    {
        CollectedItemsState.TryCollect("Map1_Key");
        CollectedItemsState.TryCollect("Map2_Key");
        CollectedItemsState.TryCollect("Map3_Key");
        CollectedItemsState.TryCollect("Extra_Item");

        Assert.IsTrue(CollectedItemsState.HasAllRequiredItems);
    }

    [Test]
    public void ResetProgress_ClearsItems()
    {
        CollectedItemsState.TryCollect("Map1_Key");

        CollectedItemsState.ResetProgress();

        Assert.AreEqual(0, CollectedItemsState.CollectedCount);
        Assert.IsFalse(CollectedItemsState.IsCollected("Map1_Key"));
    }
}