using UnityEngine;

[RequireComponent(typeof(CircleVisual))]
public sealed class PlayerPolish : MonoBehaviour
{
    [SerializeField] private CircleVisual circleVisual;
    [SerializeField] private float hueCycleSpeed = 0.15f;
    [SerializeField] private float colorLerpSpeed = 8f;
    [SerializeField] private float idlePulseAmplitude = 0.04f;
    [SerializeField] private float idlePulseFrequency = 3f;
    [SerializeField] private float tapPulseAmount = 0.16f;
    [SerializeField] private float tapPulseDecay = 10f;
    [SerializeField] private float trailTime = 0.35f;
    [SerializeField] private float trailWidth = 0.14f;

    private TrailRenderer trailRenderer;
    private Color currentColor = Color.white;
    private Vector3 baseScale;
    private float tapPulse;

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
        tapPulse = Mathf.MoveTowards(tapPulse, 0f, tapPulseDecay * Time.deltaTime);
        var idlePulse = 1f + Mathf.Sin(Time.time * idlePulseFrequency) * idlePulseAmplitude;
        transform.localScale = baseScale * (idlePulse + tapPulse);
        ApplyColor(currentColor);
    }

    public void PlayTapPulse() => tapPulse = tapPulseAmount;

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
}
