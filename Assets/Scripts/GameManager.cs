using System;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> ScoreChanged;
    public event Action<bool> GameStateChanged;
    public event Action Died;

    public bool IsPlaying { get; private set; }

    public bool IsGameOver => !IsPlaying;

    public int Score => Mathf.FloorToInt(score);

    private float score;
    private int lastPublishedScore = -1;
    private OrbitController orbitController;
    private ObstacleSpawner obstacleSpawner;
    private DeathImpactFeedback deathImpactFeedback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
        orbitController = FindFirstObjectByType<OrbitController>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        deathImpactFeedback = FindFirstObjectByType<DeathImpactFeedback>();
        if (GetComponent<CosmeticProgressionManager>() == null)
        {
            gameObject.AddComponent<CosmeticProgressionManager>();
        }
    }

    private void Start() => StartGame();

    private void Update()
    {
        if (IsPlaying)
        {
            score += Time.deltaTime;
            PublishScoreIfNeeded();
        }
        else if (WasRestartPressed())
        {
            RestartGame();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void StartGame()
    {
        score = 0f;
        lastPublishedScore = -1;
        IsPlaying = true;
        PublishScoreIfNeeded();
        GameStateChanged?.Invoke(true);
    }

    public void GameOver()
    {
        if (!IsPlaying)
        {
            return;
        }

        IsPlaying = false;
        Died?.Invoke();
        GameStateChanged?.Invoke(false);
    }

    public void AddBonusScore(int amount)
    {
        score += Mathf.Max(0, amount);
        PublishScoreIfNeeded();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        deathImpactFeedback ??= FindFirstObjectByType<DeathImpactFeedback>();
        obstacleSpawner ??= FindFirstObjectByType<ObstacleSpawner>();
        orbitController ??= FindFirstObjectByType<OrbitController>();

        deathImpactFeedback?.ResetState();
        obstacleSpawner?.ResetState();
        orbitController?.ResetState();
        StartGame();
    }

    private void PublishScoreIfNeeded()
    {
        var currentScore = Score;
        if (currentScore == lastPublishedScore)
        {
            return;
        }

        lastPublishedScore = currentScore;
        ScoreChanged?.Invoke(currentScore);
    }

    private static bool WasRestartPressed()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }

        return Input.GetMouseButtonDown(0);
    }
}
