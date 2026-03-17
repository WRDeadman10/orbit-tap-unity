using UnityEngine;

public sealed class ObstacleController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float despawnRadius = 0.35f;
    [SerializeField, Min(0.1f)] private float nearMissRadiusWindow = 0.65f;
    [SerializeField, Min(0.1f)] private float nearMissDistance = 1f;
    [SerializeField, Min(0.1f)] private float collisionBuffer = 0.4f;

    private Transform orbitCenter;
    private ObstacleSpawner owner;
    private BoxVisual boxVisual;
    private Transform playerTransform;
    private NearMissFeedback nearMissFeedback;
    private float moveSpeed;
    private float rotationSpeed;
    private bool isActive;
    private bool nearMissTriggered;

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
        TryTriggerNearMiss();

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
        Color color,
        Transform nextPlayerTransform,
        NearMissFeedback nextNearMissFeedback)
    {
        owner = nextOwner;
        orbitCenter = nextCenter;
        playerTransform = nextPlayerTransform;
        nearMissFeedback = nextNearMissFeedback;
        moveSpeed = nextMoveSpeed;
        rotationSpeed = nextRotationSpeed;
        isActive = true;
        nearMissTriggered = false;
        boxVisual ??= GetComponent<BoxVisual>();

        var radians = angle * Mathf.Deg2Rad;
        var direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f);
        transform.position = orbitCenter.position + direction * radius;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        if (boxVisual != null)
        {
            boxVisual.SetAppearance(size, color);
        }
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

    public void ForceReset()
    {
        isActive = false;
        nearMissTriggered = false;
        gameObject.SetActive(false);
    }

    private void TryTriggerNearMiss()
    {
        if (nearMissTriggered || playerTransform == null || nearMissFeedback == null)
        {
            return;
        }

        var playerOrbitRadius = Vector2.Distance(playerTransform.position, orbitCenter.position);
        var obstacleRadius = Vector2.Distance(transform.position, orbitCenter.position);
        if (Mathf.Abs(playerOrbitRadius - obstacleRadius) > nearMissRadiusWindow)
        {
            return;
        }

        var distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer >= nearMissDistance || distanceToPlayer <= collisionBuffer)
        {
            return;
        }

        nearMissTriggered = true;
        nearMissFeedback.Trigger();
    }
}
