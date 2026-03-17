using UnityEngine;

public sealed class OrbitController : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField, Min(0.1f)] private float radius = 2.5f;
    [SerializeField, Min(1f)] private float speed = 180f;
    [SerializeField] private int direction = 1;
    [SerializeField] private float startAngle;

    private float angle;

    public float Radius => radius;

    public int Direction => direction >= 0 ? 1 : -1;

    private void Awake()
    {
        angle = startAngle;
        SnapToOrbit();
    }

    private void Update()
    {
        if (orbitCenter == null)
        {
            return;
        }

        angle += speed * Direction * Time.deltaTime;
        SnapToOrbit();
    }

    private void OnValidate()
    {
        direction = Direction;
        radius = Mathf.Max(0.1f, radius);
        speed = Mathf.Max(1f, speed);

        if (!Application.isPlaying)
        {
            angle = startAngle;
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
        var offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * radius;
        transform.position = orbitCenter.position + offset;
    }
}
