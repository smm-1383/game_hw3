using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{
    private enum State
    {
        Entering,
        InFormation,
        Diving,
        Returning,
        Minion
    }

    [SerializeField] private float wobbleSpeed = 8f;
    [SerializeField] private float wobbleScale = 0.08f;
    [SerializeField] private float wobbleAngle = 6f;
    [SerializeField] private float swayBlendTime = 1f;
    [SerializeField] private int curveSteps = 12;

    private readonly List<Vector3> curve = new List<Vector3>();
    private State state;
    private Vector3 baseScale;
    private Vector3 slot;
    private Vector3 diveDirection;
    private int curveIndex;
    private float speed;
    private float diveSpeed;
    private float eggMin;
    private float eggMax;
    private float eggTimer;
    private float aimChance;
    private float diveChance;
    private float diveInterval;
    private float diveTimer;
    private float swayAmount;
    private float swaySpeed;
    private float formationTime;
    private float wobblePhase;

    protected override void Awake()
    {
        base.Awake();
        baseScale = transform.localScale;
    }

    public void Init(EnemyDataSO enemyData, WaveDataSO wave, RoundEntry entry, EntryPathSO path, Vector3 slotPosition)
    {
        Setup(enemyData, Mathf.Max(1, Mathf.RoundToInt(enemyData.maxHp * entry.hpMul)));
        slot = slotPosition;
        speed = enemyData.speed * entry.speedMul;
        diveSpeed = enemyData.diveSpeed * entry.speedMul;
        float fireMul = enemyData.fireRateMul * entry.fireRateMul;
        eggMin = wave.eggMin / fireMul;
        eggMax = wave.eggMax / fireMul;
        aimChance = entry.aimChance;
        diveChance = Mathf.Min(0.95f, wave.diveChance * entry.diveMul);
        diveInterval = wave.diveInterval / entry.diveMul;
        swayAmount = wave.swayAmount;
        swaySpeed = wave.swaySpeed;
        path.FillCurve(curve, slot, curveSteps);
        curveIndex = 0;
        transform.position = curve[0];
        state = State.Entering;
        StartTimers();
    }

    public void InitMinion(EnemyDataSO enemyData, int startHp)
    {
        Setup(enemyData, startHp);
        diveSpeed = enemyData.diveSpeed;
        Vector3 direction = Vector3.down;
        if (PlayerController.Instance != null)
        {
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
        }
        diveDirection = direction;
        state = State.Minion;
        StartTimers();
    }

    private void StartTimers()
    {
        formationTime = 0f;
        wobblePhase = Random.value * Mathf.PI * 2f;
        eggTimer = Random.Range(eggMin, eggMax);
        diveTimer = Random.Range(diveInterval * 0.5f, diveInterval * 1.5f);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        transform.localScale = baseScale;
        transform.rotation = Quaternion.identity;
    }

    protected override void Update()
    {
        base.Update();
        switch (state)
        {
            case State.Entering:
                FollowCurve();
                TickEgg();
                break;
            case State.InFormation:
                UpdateFormation();
                break;
            case State.Diving:
                UpdateDive();
                break;
            case State.Returning:
                UpdateReturn();
                break;
            case State.Minion:
                UpdateMinion();
                break;
        }
        Wobble();
    }

    private void FollowCurve()
    {
        transform.position = Vector3.MoveTowards(transform.position, curve[curveIndex], speed * Time.deltaTime);
        if (transform.position == curve[curveIndex])
        {
            curveIndex++;
        }
        if (curveIndex >= curve.Count)
        {
            state = State.InFormation;
        }
    }

    private Vector3 SlotPosition()
    {
        float blend = Mathf.Clamp01(formationTime / swayBlendTime);
        float offset = Mathf.Sin(Time.time * swaySpeed) * swayAmount * blend;
        return slot + new Vector3(offset, 0f, 0f);
    }

    private void UpdateFormation()
    {
        formationTime += Time.deltaTime;
        transform.position = SlotPosition();
        TickEgg();
        diveTimer -= Time.deltaTime;
        if (diveTimer <= 0f)
        {
            diveTimer = Random.Range(diveInterval * 0.5f, diveInterval * 1.5f);
            if (Random.value < diveChance)
            {
                StartDive();
            }
        }
    }

    private void TickEgg()
    {
        eggTimer -= Time.deltaTime;
        if (eggTimer <= 0f)
        {
            DropEgg();
        }
    }

    private void DropEgg()
    {
        eggTimer = Random.Range(eggMin, eggMax);
        if (data.eggPrefab != null)
        {
            Egg egg = PoolManager.Instance.Get(data.eggPrefab, transform.position, Quaternion.identity);
            if (PlayerController.Instance != null && Random.value < aimChance)
            {
                egg.Aim(PlayerController.Instance.transform.position);
            }
        }
    }

    private void StartDive()
    {
        Vector3 target = transform.position + Vector3.down;
        if (PlayerController.Instance != null)
        {
            target = PlayerController.Instance.transform.position;
        }
        Vector3 direction = (target - transform.position).normalized;
        direction.y = Mathf.Min(direction.y, -0.6f);
        diveDirection = direction.normalized;
        state = State.Diving;
    }

    private void UpdateDive()
    {
        transform.position += diveDirection * diveSpeed * Time.deltaTime;
        if (transform.position.y < ScreenArea.Bottom - 1f)
        {
            transform.position = new Vector3(slot.x, ScreenArea.Top + 1f, 0f);
            state = State.Returning;
        }
    }

    private void UpdateReturn()
    {
        Vector3 target = SlotPosition();
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target)
        {
            formationTime = 0f;
            state = State.InFormation;
        }
    }

    private void UpdateMinion()
    {
        transform.position += diveDirection * diveSpeed * Time.deltaTime;
        Vector3 position = transform.position;
        bool outside = position.y < ScreenArea.Bottom - 1f || position.x < ScreenArea.Left - 1f || position.x > ScreenArea.Right + 1f;
        if (outside)
        {
            Remove();
        }
    }

    private void Wobble()
    {
        float wave = Mathf.Sin(Time.time * wobbleSpeed + wobblePhase);
        transform.localScale = new Vector3(baseScale.x * (1f + wave * wobbleScale), baseScale.y * (1f - wave * wobbleScale), baseScale.z);
        transform.rotation = Quaternion.Euler(0f, 0f, wave * wobbleAngle);
    }
}
