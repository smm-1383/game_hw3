using System;

public static class GameEvents
{
    public static Action<int> ScoreChanged;
    public static Action<int> LivesChanged;
    public static Action<int> PowerChanged;
    public static Action<int> MissilesChanged;
    public static Action<float> FoodChanged;
    public static Action PlayerDamaged;
    public static Action<int, int, int> RoundStarted;
    public static Action RoundCleared;
    public static Action<Boss> BossSpawned;
    public static Action<float> BossHealthChanged;
    public static Action<int> LevelCompleted;
    public static Action GameOver;
    public static Action Victory;
    public static Action<GameState> StateChanged;

    public static void Clear()
    {
        ScoreChanged = null;
        LivesChanged = null;
        PowerChanged = null;
        MissilesChanged = null;
        FoodChanged = null;
        PlayerDamaged = null;
        RoundStarted = null;
        RoundCleared = null;
        BossSpawned = null;
        BossHealthChanged = null;
        LevelCompleted = null;
        GameOver = null;
        Victory = null;
        StateChanged = null;
    }
}
