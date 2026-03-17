using System.Collections.Generic;
using UnityEngine;

public sealed class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform orbitCenter;
    [SerializeField, Min(0.1f)] private float spawnInterval = 1f;
    [SerializeField, Min(0.1f)] private float spawnRadius = 7.5f;
    [SerializeField, Min(1)] private int initialPoolSize = 8;
    [SerializeField, Min(0.1f)] private float moveSpeed = 2.6f;
    [SerializeField] private Vector2 obstacleWidthRange = new(0.35f, 0.45f);
    [SerializeField] private Vector2 obstacleLengthRange = new(1.6f, 2.8f);
    [SerializeField] private Vector2 rotationSpeedRange = new(-45f, 45f);
    [SerializeField] private Color obstacleColor = new(0.98f, 0.33f, 0.38f, 1f);

    private readonly Queue<ObstacleController> pool = new();
    private float spawnTimer;

    private void Awake()
    {
        for (var i = 0; i < initialPoolSize; i++)
        {
            pool.Enqueue(CreateObstacle());
        }
    }

    private void Update()
    {
        if (orbitCenter == null || (GameManager.Instance != null && !GameManager.Instance.IsPlaying))
        {
            return;
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnInterval)
        {
            return;
        }

        spawnTimer = 0f;
        SpawnObstacle();
    }

    public void Release(ObstacleController obstacle) => pool.Enqueue(obstacle);

    private void SpawnObstacle()
    {
        var obstacle = pool.Count > 0 ? pool.Dequeue() : CreateObstacle();
        var angle = Random.Range(0f, 360f);
        var size = new Vector2(
            Random.Range(obstacleWidthRange.x, obstacleWidthRange.y),
            Random.Range(obstacleLengthRange.x, obstacleLengthRange.y));
        var rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);

        obstacle.Activate(this, orbitCenter, angle, spawnRadius, moveSpeed, rotationSpeed, size, obstacleColor);
    }

    private ObstacleController CreateObstacle()
    {
        var obstacleObject = new GameObject("Obstacle");
        obstacleObject.transform.SetParent(transform);
        obstacleObject.SetActive(false);
        obstacleObject.AddComponent<SpriteRenderer>();
        obstacleObject.AddComponent<BoxVisual>();
        obstacleObject.AddComponent<BoxCollider2D>().isTrigger = true;
        return obstacleObject.AddComponent<ObstacleController>();
    }
}
