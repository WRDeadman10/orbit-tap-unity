using UnityEngine;

public sealed class DeathParticle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 velocity;
    private float lifetime;
    private float elapsed;

    private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

    private void Update()
    {
        if (elapsed >= lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        elapsed += Time.unscaledDeltaTime;
        transform.position += velocity * Time.unscaledDeltaTime;
        velocity *= 0.94f;

        var alpha = 1f - elapsed / lifetime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }

    public void Activate(Vector3 position, Vector3 nextVelocity, Color color, float size, float nextLifetime)
    {
        transform.position = position;
        transform.localScale = Vector3.one;
        velocity = nextVelocity;
        lifetime = nextLifetime;
        elapsed = 0f;
        spriteRenderer.color = color;
        GetComponent<CircleVisual>().SetColor(color);
        gameObject.SetActive(true);
    }
}
