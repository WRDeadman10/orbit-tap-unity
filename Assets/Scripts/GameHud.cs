using UnityEngine;
using UnityEngine.UI;

public sealed class GameHud : MonoBehaviour
{
    [SerializeField] private float scoreScaleBump = 0.12f;
    [SerializeField] private float scoreScaleDecay = 8f;

    private Text scoreText;
    private Text messageText;
    private GameManager gameManager;
    private Vector3 scoreBaseScale;
    private float scoreBump;

    private void Awake() => BuildHud();

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            return;
        }

        gameManager.ScoreChanged += HandleScoreChanged;
        gameManager.GameStateChanged += HandleGameStateChanged;
        HandleScoreChanged(gameManager.Score);
        HandleGameStateChanged(gameManager.IsPlaying);
    }

    private void OnDestroy()
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.ScoreChanged -= HandleScoreChanged;
        gameManager.GameStateChanged -= HandleGameStateChanged;
    }

    private void Update()
    {
        scoreBump = Mathf.MoveTowards(scoreBump, 0f, scoreScaleDecay * Time.unscaledDeltaTime);
        scoreText.rectTransform.localScale = scoreBaseScale * (1f + scoreBump);
    }

    private void HandleScoreChanged(int score)
    {
        scoreText.text = $"Score {score}";
        scoreBump = scoreScaleBump;
    }

    private void HandleGameStateChanged(bool isPlaying)
    {
        messageText.text = isPlaying ? string.Empty : "Game Over\nTap to restart";
    }

    private void BuildHud()
    {
        if (gameObject.GetComponent<ScreenFlash>() == null)
        {
            gameObject.AddComponent<ScreenFlash>();
        }

        var canvasObject = new GameObject("Canvas");
        canvasObject.transform.SetParent(transform, false);

        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.AddComponent<GraphicRaycaster>();

        scoreText = CreateText("ScoreText", canvasObject.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -40f), 42, TextAnchor.MiddleCenter);
        messageText = CreateText("MessageText", canvasObject.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, 36, TextAnchor.MiddleCenter);
        scoreBaseScale = scoreText.rectTransform.localScale;
    }

    private static Text CreateText(
        string objectName,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 anchoredPosition,
        int fontSize,
        TextAnchor alignment)
    {
        var textObject = new GameObject(objectName);
        textObject.transform.SetParent(parent, false);

        var rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(640f, 160f);

        var text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }
}
