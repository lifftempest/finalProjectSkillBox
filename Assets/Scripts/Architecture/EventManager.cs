using System;

public static class EventManager
{
    public static Action PlayerDeath;
    public static Action PlayerWin;
    public static Action OnSettingsKeyPressed;

    public static void InvokePlayerDeathEvents()
    {
        PlayerDeath?.Invoke();
    }

    public static void InvokePlayerWinEvents()
    {
        PlayerWin?.Invoke();
    }

    public static void InvokeOnSettingsPressed()
    {
        OnSettingsKeyPressed?.Invoke();
    }
}