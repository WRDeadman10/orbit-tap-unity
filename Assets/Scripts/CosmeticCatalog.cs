using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Orbit Tap/Cosmetic Catalog", fileName = "CosmeticCatalog")]
public sealed class CosmeticCatalog : ScriptableObject
{
    public CosmeticItem[] items = Array.Empty<CosmeticItem>();
}

[Serializable]
public struct CosmeticItem
{
    public string id;
    public int unlockScore;
    public int coinCost;
    public float hueOffset;
    public float trailTime;
    public float trailWidth;
}
