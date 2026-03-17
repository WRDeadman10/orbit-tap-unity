using UnityEngine;

public sealed class ObstacleController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float despawnRadius = 0.35f;

    private Transform orbitCenter;
    private ObstacleSpawner owner;
    private BoxVisual boxVisual;
    private float moveSpeed;
    private float rotationSpeed;
    private bool isActive;

    private void Awake() => boxVisual = GetComponent<BoxVisual>();

    private void Update()
    {
        if (!isActive || orbitCenter == null || (GameManager.Instance != null && !GameManager.Instance.IsPlaying))
        {
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            orbitCenter.position,
            moveSpeed * Time.deltaTime);

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, orbitCenter.position) <= despawnRadius)
        {
            Release();
        }
    }

    public void Activate(
        ObstacleSpawner nextOwner,
        Transform nextCenter,
        float angle,
        float radius,
        float nextMoveSpeed,
        float nextRotationSpeed,
        Vector2 size,
        Color color)
    {
        owner = nextOwner;
        orbitCenter = nextCenter;
        moveSpeed = nextMoveSpeed;
        rotationSpeed = nextRotationSpeed;
        isActive = true;

        var radians = angle * Mathf.Deg2Rad;
        var direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        transform.position = orbitCenter.position + direction * radius;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        boxVisual.SetAppearance(size, color);
        gameObject.SetActive(true);
    }

    public void Release()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        gameObject.SetActive(false);
        owner.Release(this);
    }
}
