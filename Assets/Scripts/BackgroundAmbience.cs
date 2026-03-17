using UnityEngine;

public sealed class BackgroundAmbience : MonoBehaviour
{
    [SerializeField] private float pulseAmplitude = 0.12f;
    [SerializeField] private float pulseFrequency = 1.4f;
    [SerializeField] private float hueShiftSpeed = 0.08f;
    [SerializeField] private float rotationSpeed = 18f;

    private CircleVisual circleVisual;
    private Vector3 baseScale;

    private void Awake()
    {
        if (GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
        }

        circleVisual = GetComponent<CircleVisual>() ?? gameObject.AddComponent<CircleVisual>();
        baseScale = new Vector3(2.4f, 2.4f, 1f);
    }

    private void Update()
    {
        var pulse = 1f + Mathf.Sin(Time.time * pulseFrequency) * pulseAmplitude;
        transform.localScale = baseScale * pulse;
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        var color = Color.HSVToRGB(Mathf.Repeat(Time.time * hueShiftSpeed, 1f), 0.25f, 0.55f);
        color.a = 1f;
        circleVisual.SetColor(color);
    }
}
