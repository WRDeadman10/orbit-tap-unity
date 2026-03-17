using UnityEngine;

public sealed class CosmeticProgressionManager : MonoBehaviour
{
    private const string BestScoreKey = "progression.bestScore";
    private const string EquippedCosmeticKey = "progression.equippedCosmetic";

    private CosmeticCatalog catalog;
    private GameManager gameManager;
    private PlayerPolish playerPolish;

    private void Awake()
    {
        catalog = Resources.Load<CosmeticCatalog>("CosmeticCatalog");
        playerPolish = FindFirstObjectByType<PlayerPolish>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.Died += HandleRunEnded;
        }

        ApplyEquippedCosmetic();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.Died -= HandleRunEnded;
        }
    }

    private void HandleRunEnded()
    {
        var bestScore = Mathf.Max(PlayerPrefs.GetInt(BestScoreKey, 0), gameManager.Score);
        PlayerPrefs.SetInt(BestScoreKey, bestScore);

        var unlockedIndex = GetHighestUnlockedIndex(bestScore);
        if (unlockedIndex > PlayerPrefs.GetInt(EquippedCosmeticKey, 0))
        {
            PlayerPrefs.SetInt(EquippedCosmeticKey, unlockedIndex);
        }

        ApplyEquippedCosmetic();
    }

    private void ApplyEquippedCosmetic()
    {
        if (catalog == null || catalog.items.Length == 0 || playerPolish == null)
        {
            return;
        }

        var index = Mathf.Clamp(PlayerPrefs.GetInt(EquippedCosmeticKey, 0), 0, GetHighestUnlockedIndex(PlayerPrefs.GetInt(BestScoreKey, 0)));
        var item = catalog.items[index];
        playerPolish.SetCosmetic(item.hueOffset, item.trailTime, item.trailWidth);
    }

    private int GetHighestUnlockedIndex(int bestScore)
    {
        if (catalog == null || catalog.items.Length == 0)
        {
            return 0;
        }

        var highestIndex = 0;
        for (var i = 0; i < catalog.items.Length; i++)
        {
            if (bestScore >= catalog.items[i].unlockScore)
            {
                highestIndex = i;
            }
        }

        return highestIndex;
    }
}
