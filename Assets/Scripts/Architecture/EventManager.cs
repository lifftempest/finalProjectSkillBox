using System;

public static class EventManager
{
    public static Action PlayerDeath;

    public static void InvokePlayerDeathEvents()
    {
        PlayerDeath?.Invoke();
    }
}