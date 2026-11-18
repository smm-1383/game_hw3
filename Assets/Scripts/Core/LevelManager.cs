using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelDataSO level;
    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private RoundSpawner spawner;
    [SerializeField] private ParallaxLayer[] parallaxLayers;
    [SerializeField] private float roundIntroDelay = 1.5f;
    [SerializeField] private float roundEndDelay = 1f;
    [SerializeField] private float levelCompleteDelay = 3f;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }

    private void OnEnable()
    {
        GameEvents.GameOver += StopLevel;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= StopLevel;
    }

    private void Start()
    {
        ApplyTheme();
        StartCoroutine(RunLevel());
    }

    private void ApplyTheme()
    {
        ThemeData theme = level.theme;
        int count = Mathf.Min(parallaxLayers.Length, theme.layerSprites.Length);
        for (int i = 0; i < count; i++)
        {
            parallaxLayers[i].Apply(theme.layerSprites[i], theme.layerSpeeds[i], theme.tint);
        }
    }

    private void StopLevel()
    {
        StopAllCoroutines();
        spawner.StopRound();
    }

    private IEnumerator RunLevel()
    {
        GameManager game = GameManager.Instance;
        game.EnterLevel(level);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(level.music);
        }
        game.RaiseAll();
        int total = level.rounds.Length;
        for (int i = game.RoundIndex; i < total; i++)
        {
            game.RoundIndex = i;
            GameEvents.RoundStarted?.Invoke(level.levelNumber, i + 1, total);
            SaveSystem.Save(game.CreateSave());
            yield return new WaitForSeconds(roundIntroDelay);
            spawner.StartRound(level.rounds[i]);
            yield return new WaitUntil(() => spawner.IsCleared);
            GameEvents.RoundCleared?.Invoke();
            if (i + 1 < total)
            {
                game.RoundIndex = i + 1;
                SaveSystem.Save(game.CreateSave());
            }
            yield return new WaitForSeconds(roundEndDelay);
        }
        game.SetState(GameState.LevelComplete);
        GameEvents.LevelCompleted?.Invoke(level.levelNumber);
        yield return new WaitForSeconds(levelCompleteDelay);
        game.CompleteLevel();
    }
}
