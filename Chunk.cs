using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Chunk
{
    public List<GameObject> blocks = new List<GameObject>();
    public List<Vector2> blocksPos = new List<Vector2>();

    public Vector2 chunkCoord;

    public GameObject parentObj;

    public int chunkID;

}
