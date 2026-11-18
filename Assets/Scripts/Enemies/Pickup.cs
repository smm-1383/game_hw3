using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 3f;

    protected abstract void Collect();

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        if (transform.position.y < ScreenArea.Bottom - 1f)
        {
            PoolManager.Instance.Release(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            Collect();
            PoolManager.Instance.Release(gameObject);
        }
    }
}
