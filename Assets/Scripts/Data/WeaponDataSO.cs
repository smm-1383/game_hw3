using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ChickenShooter/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public Bullet bulletPrefab;
    [ColorUsage(true, true)] public Color bulletColor = Color.white;
    public int damage = 1;
    public float bulletSpeed = 14f;
    public float fireRate = 5f;
    public FirePattern pattern;
    public float spreadAngle = 12f;
    public float bossDamagePercent = 0.18f;
}
