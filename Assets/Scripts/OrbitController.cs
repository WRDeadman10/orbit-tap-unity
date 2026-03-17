using UnityEngine;

public sealed class OrbitController : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField, Min(0.1f)] private float radius = 2.5f;
    [SerializeField, Min(1f)] private float speed = 180f;
    [SerializeField, Min(0.01f)] private float radiusSmoothTime = 0.08f;
    [SerializeField, Min(1f)] private float turnAcceleration = 1440f;
    [SerializeField, Min(1f)] private float turnBoostMultiplier = 1.12f;
    [SerializeField] private int direction = 1;
    [SerializeField] private float startAngle;

    private float angle;
    private float currentRadius;
    private float radiusVelocity;
    private float currentAngularSpeed;
    private float initialRadius;
    private int initialDirection;

    public float Radius => radius;

    public int Direction => direction >= 0 ? 1 : -1;

    private void Awake()
    {
        initialRadius = radius;
        initialDirection = Direction;
        angle = startAngle;
        currentRadius = radius;
        currentAngularSpeed = speed * Direction;
        SnapToOrbit();
    }

    private void Update()
    {
        if (orbitCenter == null || (GameManager.Instance != null && !GameManager.Instance.IsPlaying))
        {
            return;
        }

        var targetAngularSpeed = speed * Direction;
        currentAngularSpeed = Mathf.MoveTowards(currentAngularSpeed, targetAngularSpeed, turnAcceleration * Time.deltaTime);
        angle += currentAngularSpeed * Time.deltaTime;
        currentRadius = Mathf.SmoothDamp(currentRadius, radius, ref radiusVelocity, radiusSmoothTime);
        SnapToOrbit();
    }

    private void OnValidate()
    {
        direction = Direction;
        radius = Mathf.Max(0.1f, radius);
        speed = Mathf.Max(1f, speed);
        radiusSmoothTime = Mathf.Max(0.01f, radiusSmoothTime);
        turnAcceleration = Mathf.Max(1f, turnAcceleration);
        turnBoostMultiplier = Mathf.Max(1f, turnBoostMultiplier);

        if (!Application.isPlaying)
        {
            angle = startAngle;
            currentRadius = radius;
            currentAngularSpeed = speed * Direction;
            SnapToOrbit();
        }
    }

    public void SwitchDirection()
    {
        direction *= -1;
        currentAngularSpeed = speed * direction * turnBoostMultiplier;
    }

    public void SetRadius(float nextRadius)
    {
        radius = Mathf.Max(0.1f, nextRadius);
        SnapToOrbit();
    }

    public void ResetState()
    {
        direction = initialDirection;
        radius = initialRadius;
        angle = startAngle;
        radiusVelocity = 0f;
        currentRadius = radius;
        currentAngularSpeed = speed * Direction;
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
