using UnityEngine;

[RequireComponent(typeof(CircleVisual))]
public sealed class PlayerPolish : MonoBehaviour
{
    [SerializeField] private CircleVisual circleVisual;
    [SerializeField] private float hueCycleSpeed = 0.15f;
    [SerializeField] private float colorLerpSpeed = 8f;
    [SerializeField] private float idlePulseAmplitude = 0.035f;
    [SerializeField] private float idlePulseFrequency = 2.8f;
    [SerializeField] private Vector2 tapSquashStretch = new(1.18f, 0.82f);
    [SerializeField] private float tapRecoveryDuration = 0.18f;
    [SerializeField] private float tapRecoveryOvershoot = 1.9f;
    [SerializeField] private float trailTime = 0.28f;
    [SerializeField] private float trailWidth = 0.16f;

    private TrailRenderer trailRenderer;
    private Material trailMaterial;
    private Color currentColor = Color.white;
    private Vector3 baseScale;
    private Vector3 tapScale = Vector3.one;
    private float tapRecoveryTime = 1f;
    private float cosmeticHueOffset;

    private void Awake()
    {
        circleVisual ??= GetComponent<CircleVisual>();
        baseScale = transform.localScale;
        ConfigureTrail();
        ApplyColor(currentColor);
    }

    private void Update()
    {
        var targetColor = Color.HSVToRGB(Mathf.Repeat(Time.time * hueCycleSpeed + cosmeticHueOffset, 1f), 0.72f, 1f);
        currentColor = Color.Lerp(currentColor, targetColor, colorLerpSpeed * Time.deltaTime);
        UpdateScale();
        ApplyColor(currentColor);
    }

    public void PlayTapPulse()
    {
        tapScale = new Vector3(tapSquashStretch.x, tapSquashStretch.y, 1f);
        tapRecoveryTime = 0f;
    }

    public void SetCosmetic(float hueOffset, float nextTrailTime, float nextTrailWidth)
    {
        cosmeticHueOffset = hueOffset;
        trailTime = nextTrailTime;
        trailWidth = nextTrailWidth;
        ConfigureTrail();
    }

    private void ConfigureTrail()
    {
        if (!EnsureTrailRenderer())
        {
            return;
        }

        trailRenderer.time = trailTime;
        trailRenderer.startWidth = trailWidth;
        trailRenderer.endWidth = 0f;
        trailRenderer.minVertexDistance = 0.05f;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trailRenderer.receiveShadows = false;
        trailRenderer.numCapVertices = 6;
        trailRenderer.alignment = LineAlignment.View;
        trailMaterial ??= new Material(Shader.Find("Sprites/Default"));
        trailRenderer.material = trailMaterial;
        trailRenderer.emitting = true;
        trailRenderer.colorGradient = CreateTrailGradient();
    }

    private void ApplyColor(Color color)
    {
        circleVisual.SetColor(color);
        if (EnsureTrailRenderer())
        {
            trailRenderer.colorGradient = CreateTrailGradient(color);
        }
    }

    private void UpdateScale()
    {
        if (tapRecoveryTime < 1f)
        {
            tapRecoveryTime += Time.deltaTime / tapRecoveryDuration;
            var eased = EaseOutBack(Mathf.Clamp01(tapRecoveryTime), tapRecoveryOvershoot);
            tapScale = Vector3.LerpUnclamped(tapScale, Vector3.one, eased);
        }
        else
        {
            tapScale = Vector3.one;
        }

        var idlePulse = 1f + Mathf.Sin(Time.time * idlePulseFrequency) * idlePulseAmplitude;
        transform.localScale = Vector3.Scale(baseScale * idlePulse, tapScale);
    }

    private static float EaseOutBack(float value, float overshoot)
    {
        var inverse = value - 1f;
        return 1f + (overshoot + 1f) * inverse * inverse * inverse + overshoot * inverse * inverse;
    }

    private static Gradient CreateTrailGradient() => CreateTrailGradient(new Color(0.25f, 0.95f, 1f, 1f));

    private static Gradient CreateTrailGradient(Color leadColor)
    {
        Color.RGBToHSV(leadColor, out var hue, out _, out _);
        var midColor = Color.HSVToRGB(Mathf.Repeat(hue + 0.08f, 1f), 0.7f, 1f);
        var endColor = Color.HSVToRGB(Mathf.Repeat(hue + 0.16f, 1f), 0.55f, 1f);

        var gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(leadColor, 0f),
                new GradientColorKey(midColor, 0.45f),
                new GradientColorKey(endColor, 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.9f, 0f),
                new GradientAlphaKey(0.35f, 0.55f),
                new GradientAlphaKey(0f, 1f)
            });
        return gradient;
    }

    private bool EnsureTrailRenderer()
    {
        if (trailRenderer != null)
        {
            return true;
        }

        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
        {
            trailRenderer = gameObject.AddComponent<TrailRenderer>();
        }

        return trailRenderer != null;
    }
}
