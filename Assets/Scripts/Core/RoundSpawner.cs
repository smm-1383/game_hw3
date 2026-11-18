using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSpawner : MonoBehaviour
{
    private int alive;
    private bool spawnDone;

    public bool IsCleared
    {
        get { return spawnDone && alive == 0; }
    }

    public void StartRound(RoundEntry entry)
    {
        alive = 0;
        spawnDone = false;
        switch (entry.wave.waveType)
        {
            case WaveType.Chickens:
                StartCoroutine(SpawnChickens(entry));
                break;
            case WaveType.Asteroids:
                StartCoroutine(SpawnAsteroids(entry));
                break;
            case WaveType.Boss:
                SpawnBoss(entry);
                break;
        }
    }

    public void StopRound()
    {
        StopAllCoroutines();
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        alive--;
    }

    private IEnumerator SpawnChickens(RoundEntry entry)
    {
        WaveDataSO wave = entry.wave;
        int count = Mathf.Max(1, Mathf.RoundToInt(wave.count * entry.countMul));
        List<Vector2> slots = FormationSlots(wave, count);
        Vector3 center = ScreenArea.FromViewport(0.5f, wave.formationTop);
        for (int i = 0; i < count; i++)
        {
            EntryPathSO path = wave.entryPaths[i % wave.entryPaths.Length];
            Vector3 slot = center + (Vector3)slots[i];
            Chicken chicken = PoolManager.Instance.Get(wave.chickenPrefab, Vector3.zero, Quaternion.identity);
            chicken.Init(wave.enemy, wave, entry, path, slot);
            chicken.Removed += OnEnemyRemoved;
            alive++;
            yield return new WaitForSeconds(wave.entryDelay);
        }
        spawnDone = true;
    }

    private IEnumerator SpawnAsteroids(RoundEntry entry)
    {
        WaveDataSO wave = entry.wave;
        int count = Mathf.Max(1, Mathf.RoundToInt(wave.asteroidCount * entry.countMul));
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(ScreenArea.Left + 1f, ScreenArea.Right - 1f);
            Vector3 position = new Vector3(x, ScreenArea.Top + 1.5f, 0f);
            float scale = Random.Range(wave.asteroidScaleMin, wave.asteroidScaleMax);
            float size = Mathf.InverseLerp(wave.asteroidScaleMin, wave.asteroidScaleMax, scale);
            int hp = Mathf.Max(1, Mathf.RoundToInt(Mathf.Lerp(wave.asteroidHpMin, wave.asteroidHpMax, size) * entry.hpMul));
            float speed = Random.Range(wave.asteroidSpeedMin, wave.asteroidSpeedMax) * entry.speedMul;
            Asteroid asteroid = PoolManager.Instance.Get(wave.asteroidPrefab, position, Quaternion.identity);
            asteroid.Init(wave.enemy, hp, wave, scale, speed);
            asteroid.Removed += OnEnemyRemoved;
            alive++;
            yield return new WaitForSeconds(wave.asteroidSpawnInterval);
        }
        spawnDone = true;
    }

    private void SpawnBoss(RoundEntry entry)
    {
        Boss boss = PoolManager.Instance.Get(entry.wave.bossPrefab, Vector3.zero, Quaternion.identity);
        boss.Begin(entry.hpMul);
        boss.Removed += OnEnemyRemoved;
        alive = 1;
        spawnDone = true;
    }

    private List<Vector2> FormationSlots(WaveDataSO wave, int count)
    {
        switch (wave.formation)
        {
            case FormationType.VShape:
                return VSlots(count, wave.spacing);
            case FormationType.Diamond:
                return DiamondSlots(count, wave.spacing);
            case FormationType.Circle:
                return CircleSlots(count, wave.spacing);
            case FormationType.Compact:
                return GridSlots(count, wave.cols, wave.spacing * 0.7f);
            default:
                return GridSlots(count, wave.cols, wave.spacing);
        }
    }

    private List<Vector2> GridSlots(int count, int cols, Vector2 spacing)
    {
        List<Vector2> slots = new List<Vector2>();
        cols = Mathf.Max(1, cols);
        for (int i = 0; i < count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            int inRow = Mathf.Min(cols, count - row * cols);
            float x = (col - (inRow - 1) * 0.5f) * spacing.x;
            slots.Add(new Vector2(x, -row * spacing.y));
        }
        return slots;
    }

    private List<Vector2> VSlots(int count, Vector2 spacing)
    {
        List<Vector2> slots = new List<Vector2>();
        int deepest = count / 2;
        for (int i = 0; i < count; i++)
        {
            int depth = (i + 1) / 2;
            int side = i % 2 == 1 ? -1 : 1;
            slots.Add(new Vector2(side * depth * spacing.x, (depth - deepest) * spacing.y));
        }
        return slots;
    }

    private List<Vector2> DiamondSlots(int count, Vector2 spacing)
    {
        List<Vector2Int> cells = new List<Vector2Int>();
        int radius = 0;
        while (cells.Count < count)
        {
            cells.Clear();
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
                    {
                        cells.Add(new Vector2Int(x, y));
                    }
                }
            }
            radius++;
        }
        cells.Sort(CompareCells);
        List<Vector2> slots = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            slots.Add(new Vector2(cells[i].x * spacing.x, (cells[i].y - radius) * spacing.y));
        }
        return slots;
    }

    private int CompareCells(Vector2Int a, Vector2Int b)
    {
        int distance = (Mathf.Abs(a.x) + Mathf.Abs(a.y)).CompareTo(Mathf.Abs(b.x) + Mathf.Abs(b.y));
        if (distance != 0)
        {
            return distance;
        }
        return a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y);
    }

    private List<Vector2> CircleSlots(int count, Vector2 spacing)
    {
        List<Vector2> slots = new List<Vector2>();
        float radius = Mathf.Max(spacing.x, count * spacing.x / (2f * Mathf.PI));
        for (int i = 0; i < count; i++)
        {
            float angle = i * 2f * Mathf.PI / count;
            slots.Add(new Vector2(Mathf.Cos(angle) * radius, (Mathf.Sin(angle) - 1f) * radius * 0.6f));
        }
        return slots;
    }
}
