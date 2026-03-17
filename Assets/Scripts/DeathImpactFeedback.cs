using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DeathImpactFeedback : MonoBehaviour
{
    [SerializeField] private int particleCount = 10;
    [SerializeField] private float particleSpeed = 5f;
    [SerializeField] private float particleLifetime = 0.45f;
    [SerializeField] private float slowMotionScale = 0.2f;
    [SerializeField] private float slowMotionDuration = 0.05f;
    [SerializeField, Range(0f, 1f)] private float deathShakeIntensity = 0.8f;

    private readonly Queue<DeathParticle> pool = new();
    private CameraShake cameraShake;
    private GameManager gameManager;

    private void Awake()
    {
        cameraShake = Camera.main != null ? Camera.main.GetComponent<CameraShake>() : null;

        for (var i = 0; i < particleCount; i++)
        {
            pool.Enqueue(CreateParticle());
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.Died += HandleDeath;
        }
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.Died -= HandleDeath;
        }
    }

    private void HandleDeath()
    {
        cameraShake?.Shake(deathShakeIntensity);
        EmitBurst();
        StartCoroutine(PlaySlowMotion());
    }

    private void EmitBurst()
    {
        for (var i = 0; i < particleCount; i++)
        {
            var angle = (360f / particleCount) * i;
            var direction = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            var particle = pool.Count > 0 ? pool.Dequeue() : CreateParticle();
            var tint = Color.HSVToRGB(Random.value, 0.5f, 1f);
            particle.Activate(transform.position, direction * Random.Range(particleSpeed * 0.7f, particleSpeed), tint, 0.12f, particleLifetime);
            StartCoroutine(ReturnWhenDone(particle));
        }
    }

    private IEnumerator PlaySlowMotion()
    {
        Time.timeScale = slowMotionScale;
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1f;
    }

    private IEnumerator ReturnWhenDone(DeathParticle particle)
    {
        yield return new WaitForSecondsRealtime(particleLifetime + 0.02f);
        if (!pool.Contains(particle))
        {
            pool.Enqueue(particle);
        }
    }

    private DeathParticle CreateParticle()
    {
        var particleObject = new GameObject("DeathParticle");
        particleObject.transform.SetParent(transform);
        particleObject.SetActive(false);
        particleObject.AddComponent<SpriteRenderer>();
        particleObject.AddComponent<CircleVisual>();
        return particleObject.AddComponent<DeathParticle>();
    }
}
