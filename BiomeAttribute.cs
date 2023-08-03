using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BiomeAttribute", menuName = "LampAdventure/BiomeData")]
public class BiomeAttribute : ScriptableObject
{

    [Header("Biome General Data")]
    public string biomeName;

    public List<int> blockTypes = new List<int>();
    /* blockTypes :
    0 = stone-like
    1 = grass-like
    2 = crystal
    3 = ore
    */
    [Header("Procedural Generation Data")]
    public float biomeSeed;
    public float perlinScale = 0.3f;
    public float perlinThreshold = 0.45f;

}

[System.Serializable]
public class Block
{
    public string blockName;

    public GameObject blockObject;

    public Sprite icon;

}
