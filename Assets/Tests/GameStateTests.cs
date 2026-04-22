using NUnit.Framework;

public class GameStateTests
{
    [SetUp]
    public void Setup()
    {
        GameState.powerOn = false;
    }

    [Test]
    public void DefaultState_IsFalse()
    {
        Assert.IsFalse(GameState.powerOn,
            "A powerOn alapból nem false!");
    }

    [Test]
    public void CanSetPowerOn_ToTrue()
    {
        GameState.powerOn = true;

        Assert.IsTrue(GameState.powerOn,
            "Nem sikerült true-ra állítani!");
    }

    [Test]
    public void CanSetPowerOn_BackToFalse()
    {
        GameState.powerOn = true;
        GameState.powerOn = false;

        Assert.IsFalse(GameState.powerOn,
            "Nem sikerült visszaállítani false-ra!");
    }

    [Test]
    public void StatePersistsBetweenAccesses()
    {
        GameState.powerOn = true;

        bool value = GameState.powerOn;

        Assert.IsTrue(value,
            "Az állapot nem maradt meg!");
    }
}