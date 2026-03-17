using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public sealed class CircleVisual : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float diameter = 0.35f;
    [SerializeField] private Color color = new(1f, 0.83f, 0.27f, 1f);
    [SerializeField, Range(16, 128)] private int textureSize = 64;

    private SpriteRenderer spriteRenderer;
    private Sprite runtimeSprite;
    private Texture2D runtimeTexture;

    private void OnEnable() => Rebuild();

    private void OnValidate() => Rebuild();

    public void SetColor(Color nextColor)
    {
        color = nextColor;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private void OnDisable()
    {
        if (runtimeSprite != null)
        {
            DestroyImmediate(runtimeSprite);
        }

        if (runtimeTexture != null)
        {
            DestroyImmediate(runtimeTexture);
        }
    }

    private void Rebuild()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        textureSize = Mathf.Clamp(textureSize, 16, 128);

        if (runtimeSprite != null)
        {
            DestroyImmediate(runtimeSprite);
        }

        if (runtimeTexture != null)
        {
            DestroyImmediate(runtimeTexture);
        }

        runtimeTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
            hideFlags = HideFlags.HideAndDontSave
        };

        var pixels = new Color32[textureSize * textureSize];
        var radius = (textureSize - 1) * 0.5f;
        var center = new Vector2(radius, radius);
        var tint = (Color32)Color.white;

        for (var y = 0; y < textureSize; y++)
        {
            for (var x = 0; x < textureSize; x++)
            {
                var index = x + y * textureSize;
                var point = new Vector2(x, y);
                pixels[index] = Vector2.Distance(point, center) <= radius ? tint : new Color32(0, 0, 0, 0);
            }
        }

        runtimeTexture.SetPixels32(pixels);
        runtimeTexture.Apply(false, true);

        runtimeSprite = Sprite.Create(
            runtimeTexture,
            new Rect(0f, 0f, textureSize, textureSize),
            new Vector2(0.5f, 0.5f),
            textureSize / diameter);
        runtimeSprite.hideFlags = HideFlags.HideAndDontSave;

        spriteRenderer.sprite = runtimeSprite;
        spriteRenderer.color = color;
    }
}
