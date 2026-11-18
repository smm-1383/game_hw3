using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private WeaponDataSO[] byPowerLevel;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float doubleOffset = 0.25f;

    private float cooldown;

    public bool Firing { get; set; }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (!Firing || cooldown > 0f || GameManager.Instance.State != GameState.Playing)
        {
            return;
        }
        int index = Mathf.Clamp(GameManager.Instance.Player.power - 1, 0, byPowerLevel.Length - 1);
        WeaponDataSO weapon = byPowerLevel[index];
        cooldown = 1f / weapon.fireRate;
        Fire(weapon);
    }

    private void Fire(WeaponDataSO weapon)
    {
        switch (weapon.pattern)
        {
            case FirePattern.Single:
                Spawn(weapon, 0f, 0f);
                break;
            case FirePattern.Double:
                Spawn(weapon, -doubleOffset, 0f);
                Spawn(weapon, doubleOffset, 0f);
                break;
            case FirePattern.Spread3:
                Spawn(weapon, 0f, 0f);
                Spawn(weapon, 0f, weapon.spreadAngle);
                Spawn(weapon, 0f, -weapon.spreadAngle);
                break;
        }
    }

    private void Spawn(WeaponDataSO weapon, float offsetX, float angle)
    {
        Vector3 position = muzzle.position + new Vector3(offsetX, 0f, 0f);
        Bullet bullet = PoolManager.Instance.Get(weapon.bulletPrefab, position, Quaternion.Euler(0f, 0f, angle));
        bullet.Launch(weapon.damage, weapon.bulletSpeed, weapon.bulletColor);
    }
}
