using UnityEngine;

public sealed class OrbitController : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField, Min(0.1f)] private float radius = 2.5f;
    [SerializeField, Min(1f)] private float speed = 180f;
    [SerializeField, Min(1f)] private float radiusSmoothing = 8f;
    [SerializeField] private int direction = 1;
    [SerializeField] private float startAngle;

    private float angle;
    private float currentRadius;

    public float Radius => radius;

    public int Direction => direction >= 0 ? 1 : -1;

    private void Awake()
    {
        angle = startAngle;
        currentRadius = radius;
        SnapToOrbit();
    }

    private void Update()
    {
        if (orbitCenter == null || (GameManager.Instance != null && !GameManager.Instance.IsPlaying))
        {
            return;
        }

        angle += speed * Direction * Time.deltaTime;
        currentRadius = Mathf.Lerp(currentRadius, radius, 1f - Mathf.Exp(-radiusSmoothing * Time.deltaTime));
        SnapToOrbit();
    }

    private void OnValidate()
    {
        direction = Direction;
        radius = Mathf.Max(0.1f, radius);
        speed = Mathf.Max(1f, speed);
        radiusSmoothing = Mathf.Max(1f, radiusSmoothing);

        if (!Application.isPlaying)
        {
            angle = startAngle;
            currentRadius = radius;
            SnapToOrbit();
        }
    }

    public void SwitchDirection() => direction *= -1;

    public void SetRadius(float nextRadius)
    {
        radius = Mathf.Max(0.1f, nextRadius);
        SnapToOrbit();
    }

    private void SnapToOrbit()
    {
        if (orbitCenter == null)
        {
            return;
        }

        var radians = angle * Mathf.Deg2Rad;
        var offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * currentRadius;
        transform.position = orbitCenter.position + offset;
    }
}
