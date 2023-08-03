using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "LampAdventure/WorldData")]
public class WorldData : ScriptableObject
{
    [Header("World Generation Data")]
    public List<BiomeAttribute> biomes = new List<BiomeAttribute>();
    public float worldSeed;
    public float biomeScale = 0.005f; // plus petit pour des plus grands biomes
    public int viewDistanceInChunks = 2;

    [Header("Chunk Data")]
    public int chunkSize = 16;
    public GameObject chunkParentSource;

}
