using UnityEngine;

public sealed class PlayerCollision : MonoBehaviour
{
    private void Awake()
    {
        if (GetComponent<DeathImpactFeedback>() == null)
        {
            gameObject.AddComponent<DeathImpactFeedback>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<ObstacleController>(out _))
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
            return;
        }

        Time.timeScale = 0f;
    }
}
