using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 origin;
    private Coroutine running;

    private void Awake()
    {
        origin = transform.localPosition;
    }

    public void Shake(float duration, float magnitude)
    {
        if (running != null)
        {
            StopCoroutine(running);
        }
        running = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float fade = 1f - timer / duration;
            Vector2 offset = Random.insideUnitCircle * magnitude * fade;
            transform.localPosition = origin + new Vector3(offset.x, offset.y, 0f);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = origin;
        running = null;
    }
}
