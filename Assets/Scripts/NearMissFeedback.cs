using UnityEngine;

public sealed class NearMissFeedback : MonoBehaviour
{
    [SerializeField] private int scoreBonus = 2;
    [SerializeField, Range(0f, 1f)] private float shakeIntensity = 0.18f;

    private CameraShake cameraShake;
    private ScreenFlash screenFlash;

    private void Start()
    {
        cameraShake = Camera.main != null ? Camera.main.GetComponent<CameraShake>() : null;
        screenFlash = FindFirstObjectByType<ScreenFlash>();
    }

    public void Trigger()
    {
        cameraShake?.Shake(shakeIntensity);
        screenFlash?.FlashNearMiss();
        GameManager.Instance?.AddBonusScore(scoreBonus);
    }
}
