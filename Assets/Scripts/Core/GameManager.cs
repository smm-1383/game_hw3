using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelDataSO[] levels;
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private int startLives = 3;
    [SerializeField] private int startMissiles = 2;
    [SerializeField] private int maxPower = 3;

    public int LevelIndex { get; private set; }
    public int RoundIndex { get; set; }
    public GameState State { get; private set; } = GameState.Menu;
    public PlayerState Player { get; private set; } = new PlayerState();

    private int levelStartScore;
    private int levelStartMissiles;
    private float levelStartFood;

    public LevelDataSO CurrentLevel
    {
        get { return levels[LevelIndex]; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        Player.lives = startLives;
        Player.score = 0;
        Player.missiles = startMissiles;
        Player.power = 1;
        Player.food = 0f;
    }

    public void SetState(GameState state)
    {
        State = state;
        GameEvents.StateChanged?.Invoke(state);
    }

    public void NewGame()
    {
        ResetPlayer();
        RoundIndex = 0;
        LoadLevel(0);
    }

    public void Continue()
    {
        SaveData data = SaveSystem.Load();
        if (data == null)
        {
            return;
        }
        ApplySave(data);
        LoadLevel(data.levelIndex);
    }

    public void Retry()
    {
        Continue();
    }

    public void LoadLevel(int index)
    {
        LevelIndex = index;
        Time.timeScale = 1f;
        SetState(GameState.Playing);
        SceneManager.LoadScene(levels[index].sceneName);
    }

    public void EnterLevel(LevelDataSO level)
    {
        int index = Array.IndexOf(levels, level);
        if (index >= 0)
        {
            LevelIndex = index;
        }
        if (RoundIndex == 0)
        {
            levelStartScore = Player.score;
            levelStartMissiles = Player.missiles;
            levelStartFood = Player.food;
        }
        SetState(GameState.Playing);
    }

    public void CompleteLevel()
    {
        LevelDataSO next = CurrentLevel.nextLevel;
        if (next == null)
        {
            SaveSystem.Delete();
            Time.timeScale = 0f;
            SetState(GameState.Victory);
            GameEvents.Victory?.Invoke();
            return;
        }
        RoundIndex = 0;
        LevelIndex = Array.IndexOf(levels, next);
        SaveSystem.Save(CreateSave());
        LoadLevel(LevelIndex);
    }

    public void GameOver()
    {
        SaveData data = CreateSave();
        data.roundIndex = 0;
        data.score = levelStartScore;
        data.lives = startLives;
        data.missiles = levelStartMissiles;
        data.powerLevel = 1;
        data.foodProgress = levelStartFood;
        SaveSystem.Save(data);
        Time.timeScale = 0f;
        SetState(GameState.GameOver);
        GameEvents.GameOver?.Invoke();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SetState(GameState.Menu);
        SceneManager.LoadScene(menuSceneName);
    }

    public void Pause()
    {
        if (State != GameState.Playing)
        {
            return;
        }
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void Resume()
    {
        if (State != GameState.Paused)
        {
            return;
        }
        Time.timeScale = 1f;
        SetState(GameState.Playing);
    }

    public void TogglePause()
    {
        if (State == GameState.Paused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public SaveData CreateSave()
    {
        SaveData data = new SaveData();
        data.levelIndex = LevelIndex;
        data.roundIndex = RoundIndex;
        data.score = Player.score;
        data.lives = Player.lives;
        data.missiles = Player.missiles;
        data.powerLevel = Player.power;
        data.foodProgress = Player.food;
        data.levelStartScore = levelStartScore;
        data.levelStartMissiles = levelStartMissiles;
        data.levelStartFood = levelStartFood;
        return data;
    }

    private void ApplySave(SaveData data)
    {
        LevelIndex = data.levelIndex;
        RoundIndex = data.roundIndex;
        Player.score = data.score;
        Player.lives = data.lives;
        Player.missiles = data.missiles;
        Player.power = data.powerLevel;
        Player.food = data.foodProgress;
        levelStartScore = data.levelStartScore;
        levelStartMissiles = data.levelStartMissiles;
        levelStartFood = data.levelStartFood;
    }

    public void RaiseAll()
    {
        GameEvents.ScoreChanged?.Invoke(Player.score);
        GameEvents.LivesChanged?.Invoke(Player.lives);
        GameEvents.PowerChanged?.Invoke(Player.power);
        GameEvents.MissilesChanged?.Invoke(Player.missiles);
        GameEvents.FoodChanged?.Invoke(Player.food);
    }

    public void AddScore(int amount)
    {
        Player.score += amount;
        GameEvents.ScoreChanged?.Invoke(Player.score);
    }

    public void LoseLife()
    {
        Player.lives = Mathf.Max(0, Player.lives - 1);
        GameEvents.LivesChanged?.Invoke(Player.lives);
    }

    public void AddPower(int amount)
    {
        Player.power = Mathf.Clamp(Player.power + amount, 1, maxPower);
        GameEvents.PowerChanged?.Invoke(Player.power);
    }

    public void AddMissiles(int amount)
    {
        Player.missiles += amount;
        GameEvents.MissilesChanged?.Invoke(Player.missiles);
    }

    public bool UseMissile()
    {
        if (Player.missiles <= 0)
        {
            return false;
        }
        AddMissiles(-1);
        return true;
    }

    public void AddFood(float amount)
    {
        Player.food += amount;
        if (Player.food >= 1f)
        {
            Player.food = 0f;
            AddMissiles(1);
        }
        GameEvents.FoodChanged?.Invoke(Player.food);
    }
}
