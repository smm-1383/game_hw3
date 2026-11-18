using UnityEngine;

public class SpriteFlipbook : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float framesPerSecond = 10f;
    [SerializeField] private bool faceMovement;
    [SerializeField] private float flipDeadZone = 0.002f;

    private float offset;
    private float lastX;

    private void OnEnable()
    {
        offset = Random.value * frames.Length;
        lastX = transform.position.x;
    }

    private void Update()
    {
        int index = Mathf.FloorToInt(Time.time * framesPerSecond + offset) % frames.Length;
        spriteRenderer.sprite = frames[index];
        if (!faceMovement)
        {
            return;
        }
        float moved = transform.position.x - lastX;
        lastX = transform.position.x;
        if (Mathf.Abs(moved) > flipDeadZone)
        {
            spriteRenderer.flipX = moved < 0f;
        }
    }
}
