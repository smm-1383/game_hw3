using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private WeaponDataSO missileData;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private GameObject shockwavePrefab;
    [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeMagnitude = 0.4f;

    public void Launch()
    {
        GameManager game = GameManager.Instance;
        if (game.State != GameState.Playing || !game.UseMissile())
        {
            return;
        }
        if (shockwavePrefab != null)
        {
            PoolManager.Instance.Get(shockwavePrefab, transform.position, Quaternion.identity);
        }
        if (cameraShake != null)
        {
            cameraShake.Shake(shakeDuration, shakeMagnitude);
        }
        BombRegistry.BombAll(missileData.bossDamagePercent);
    }
}
