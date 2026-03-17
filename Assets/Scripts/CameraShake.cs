using UnityEngine;

public sealed class CameraShake : MonoBehaviour
{
    [SerializeField] private float traumaDecay = 3.5f;
    [SerializeField] private float maxOffset = 0.2f;
    [SerializeField, Range(0f, 1f)] private float deathShakeIntensity = 0.65f;

    private Vector3 basePosition;
    private GameManager gameManager;
    private float trauma;

    private void Awake() => basePosition = transform.localPosition;

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

    private void LateUpdate()
    {
        if (trauma <= 0f)
        {
            transform.localPosition = basePosition;
            return;
        }

        trauma = Mathf.MoveTowards(trauma, 0f, traumaDecay * Time.unscaledDeltaTime);
        var strength = trauma * trauma;
        var offset = Random.insideUnitCircle * (maxOffset * strength);
        transform.localPosition = basePosition + new Vector3(offset.x, offset.y, 0f);
    }

    public void Shake(float intensity)
    {
        trauma = Mathf.Clamp01(Mathf.Max(trauma, intensity));
    }

    private void HandleDeath() => Shake(deathShakeIntensity);
}
