using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public sealed class BoxVisual : MonoBehaviour
{
    [SerializeField] private Vector2 size = new(0.35f, 1.75f);
    [SerializeField] private Color color = new(0.98f, 0.33f, 0.38f, 1f);

    private static Sprite sprite;
    private SpriteRenderer spriteRenderer;

    private void OnEnable() => Apply();

    private void OnValidate() => Apply();

    public void SetAppearance(Vector2 nextSize, Color nextColor)
    {
        size = nextSize;
        color = nextColor;
        Apply();
    }

    private void Apply()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        sprite ??= CreateSprite();

        transform.localScale = new Vector3(size.x, size.y, 1f);
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
    }

    private static Sprite CreateSprite()
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
            hideFlags = HideFlags.HideAndDontSave
        };

        texture.SetPixel(0, 0, Color.white);
        texture.Apply(false, true);

        var createdSprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        createdSprite.hideFlags = HideFlags.HideAndDontSave;
        return createdSprite;
    }
}
