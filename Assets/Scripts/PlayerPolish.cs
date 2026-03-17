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
    [SerializeField] private float trailTime = 0.35f;
    [SerializeField] private float trailWidth = 0.14f;

    private TrailRenderer trailRenderer;
    private Color currentColor = Color.white;
    private Vector3 baseScale;
    private Vector3 tapScale = Vector3.one;
    private float tapRecoveryTime = 1f;

    private void Awake()
    {
        circleVisual ??= GetComponent<CircleVisual>();
        trailRenderer = GetComponent<TrailRenderer>() ?? gameObject.AddComponent<TrailRenderer>();
        baseScale = transform.localScale;
        ConfigureTrail();
        ApplyColor(currentColor);
    }

    private void Update()
    {
        var targetColor = Color.HSVToRGB(Mathf.Repeat(Time.time * hueCycleSpeed, 1f), 0.72f, 1f);
        currentColor = Color.Lerp(currentColor, targetColor, colorLerpSpeed * Time.deltaTime);
        UpdateScale();
        ApplyColor(currentColor);
    }

    public void PlayTapPulse()
    {
        tapScale = new Vector3(tapSquashStretch.x, tapSquashStretch.y, 1f);
        tapRecoveryTime = 0f;
    }

    private void ConfigureTrail()
    {
        trailRenderer.time = trailTime;
        trailRenderer.startWidth = trailWidth;
        trailRenderer.endWidth = 0f;
        trailRenderer.minVertexDistance = 0.05f;
        trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trailRenderer.receiveShadows = false;
        trailRenderer.numCapVertices = 6;
        trailRenderer.alignment = LineAlignment.View;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void ApplyColor(Color color)
    {
        circleVisual.SetColor(color);
        trailRenderer.startColor = color;
        trailRenderer.endColor = new Color(color.r, color.g, color.b, 0f);
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
}
