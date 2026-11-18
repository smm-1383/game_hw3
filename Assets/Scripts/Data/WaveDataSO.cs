using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ChickenShooter/Wave Data")]
public class WaveDataSO : ScriptableObject
{
    public WaveType waveType;
    public EnemyDataSO enemy;
    public Chicken chickenPrefab;
    public int count = 10;
    public FormationType formation;
    public int cols = 5;
    public Vector2 spacing = new Vector2(1.2f, 1f);
    public float formationTop = 0.85f;
    public float swayAmount = 0.5f;
    public float swaySpeed = 1f;
    public EntryPathSO[] entryPaths;
    public float entryDelay = 0.25f;
    public float eggMin = 3f;
    public float eggMax = 6f;
    public float diveChance = 0.3f;
    public float diveInterval = 5f;
    public Asteroid asteroidPrefab;
    public int asteroidCount = 12;
    public float asteroidSpawnInterval = 0.8f;
    public int asteroidHpMin = 2;
    public int asteroidHpMax = 5;
    public float asteroidSpeedMin = 2f;
    public float asteroidSpeedMax = 4f;
    public Sprite[] asteroidSprites;
    public float asteroidScaleMin = 1f;
    public float asteroidScaleMax = 1f;
    public float asteroidSpinMin = 20f;
    public float asteroidSpinMax = 90f;
    public float asteroidDriftMax = 0.5f;
    public Boss bossPrefab;
}
