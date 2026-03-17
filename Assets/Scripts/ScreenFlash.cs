using UnityEngine;
using UnityEngine.UI;

public sealed class ScreenFlash : MonoBehaviour
{
    [SerializeField] private Color nearMissColor = new(0.25f, 0.95f, 1f, 0.18f);
    [SerializeField] private float flashDecay = 4f;

    private Image overlay;
    private Color currentColor;

    private void Awake() => BuildOverlay();

    private void Update()
    {
        if (currentColor.a <= 0f)
        {
            return;
        }

        currentColor.a = Mathf.MoveTowards(currentColor.a, 0f, flashDecay * Time.unscaledDeltaTime);
        overlay.color = currentColor;
    }

    public void FlashNearMiss()
    {
        currentColor = nearMissColor;
        overlay.color = currentColor;
    }

    private void BuildOverlay()
    {
        var overlayObject = new GameObject("ScreenFlash");
        overlayObject.transform.SetParent(transform, false);

        var rectTransform = overlayObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        overlay = overlayObject.AddComponent<Image>();
        overlay.raycastTarget = false;
        currentColor = new Color(nearMissColor.r, nearMissColor.g, nearMissColor.b, 0f);
        overlay.color = currentColor;
    }
}
