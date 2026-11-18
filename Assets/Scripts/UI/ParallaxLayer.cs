using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 1f;

    private float startY;
    private float tileHeight;
    private float scrolled;

    private void Start()
    {
        startY = transform.position.y;
        MeasureTile();
    }

    public void Apply(Sprite sprite, float layerSpeed, Color tint)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = tint;
        speed = layerSpeed;
        MeasureTile();
    }

    private void MeasureTile()
    {
        if (spriteRenderer.sprite != null)
        {
            tileHeight = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;
        }
    }

    private void Update()
    {
        if (tileHeight <= 0f)
        {
            return;
        }
        scrolled = Mathf.Repeat(scrolled + speed * Time.deltaTime, tileHeight);
        Vector3 position = transform.position;
        position.y = startY - scrolled;
        transform.position = position;
    }
}
