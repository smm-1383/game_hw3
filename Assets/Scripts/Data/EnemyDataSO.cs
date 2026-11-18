using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ChickenShooter/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public Sprite sprite;
    public Color tint = Color.white;
    public int maxHp = 1;
    public float speed = 6f;
    public float diveSpeed = 9f;
    public float fireRateMul = 1f;
    public int score = 100;
    [Range(0f, 1f)] public float foodDropChance = 0.2f;
    [Range(0f, 1f)] public float giftDropChance = 0.05f;
    public Egg eggPrefab;
    public FoodPickup foodPrefab;
    public GiftBox giftPrefab;
    public GameObject deathEffect;
}
