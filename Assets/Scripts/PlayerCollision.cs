using UnityEngine;

public sealed class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<ObstacleController>(out _))
        {
            return;
        }

        Time.timeScale = 0f;
    }
}
