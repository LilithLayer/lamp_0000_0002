using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public WorldData worldData;

    public List<Chunk> chunks = new List<Chunk>();
    List<Vector2> chunksCoord = new List<Vector2>();

    public List<Block> blocks = new List<Block>();

    public Transform player;
    Vector2 playerPosInChunks;
    Vector2 _playerPosInChunks;

    void Start ()
    {
        _playerPosInChunks.x = Mathf.FloorToInt(player.position.x / worldData.chunkSize);
        _playerPosInChunks.y = Mathf.FloorToInt(player.position.y / worldData.chunkSize);

        GenerateChunk(_playerPosInChunks, 0f);
        GenerateChunk(_playerPosInChunks - new Vector2(0, 1), 0);

        GenerateChunksAroundPlayer();
    }

    void Update ()
    {
        playerPosInChunks.x = Mathf.FloorToInt(player.position.x / worldData.chunkSize);
        playerPosInChunks.y = Mathf.FloorToInt(player.position.y / worldData.chunkSize);

        if (playerPosInChunks != _playerPosInChunks)
        {
            GenerateChunksAroundPlayer();
        }

        DestroyDistantChunk();

    }

    void LateUpdate () {
        _playerPosInChunks.x = Mathf.FloorToInt(player.position.x / worldData.chunkSize);
        _playerPosInChunks.y = Mathf.FloorToInt(player.position.y / worldData.chunkSize);
    }

    bool FindChunkWithCoord (Vector2 chunkCoord)
    {
        for (int i = 0; i < chunksCoord.Count; i++)
        {
            if (chunksCoord[i] == chunkCoord)
                return true;
        }

        return false;
    }

    public Chunk GetChunkFromVector2(Vector2 pos)
    {
        Vector2 _pos = new Vector2(pos.x, pos.y);
        _pos.x = Mathf.FloorToInt(player.position.x / worldData.chunkSize);
        _pos.y = Mathf.FloorToInt(player.position.y / worldData.chunkSize);

        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunksCoord[i] == _pos)
            {
                return chunks[i];
            }
        }

        return null;
    }

    public bool FindBlockFromVector2(Vector2 _pos)
    {
        Chunk c = GetChunkFromVector2(_pos);

        if (c.blocks.Count != 0)
        {
            for (int i = 0; i < c.blocks.Count; i++)
            {
                BlockData blockData = c.blocks[i].transform.GetComponent<BlockData>();
                if (blockData.x == (int)Mathf.FloorToInt(_pos.x) && blockData.y == (int)Mathf.FloorToInt(_pos.y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string GetBlockTypeFromVectror2(Vector2 _pos)
    {
        Chunk c = GetChunkFromVector2(_pos);

        if (c.blocks.Count != 0)
        {
            for (int i = 0; i < c.blocks.Count; i++)
            {
                BlockData blockData = c.blocks[i].transform.GetComponent<BlockData>();
                if ((blockData.x == (int)Mathf.FloorToInt(_pos.x)) && (blockData.y == (int)Mathf.FloorToInt(_pos.y)))
                {
                    return blockData.blockType;
                }
            }
        }

        return "None";
    }

    public BlockData GetBlockDataFromVector2(Vector2 _pos)
    {
        Chunk c = GetChunkFromVector2(_pos);

        if (c != null) {

            for (int i = 0; i < c.blocks.Count; i++)
            {
                BlockData blockData = c.blocks[i].transform.GetComponent<BlockData>();
                //Vector3 _blockPos = c.blocks[i].transform.position;
                if ((blockData.x == (int)Mathf.FloorToInt(_pos.x)) && (blockData.y == (int)Mathf.FloorToInt(_pos.y)))
                {
                    return c.blocks[i].GetComponent<BlockData>();
                }
            } 
        }

        return null;
    }

    Vector2 GetChunkCoordFromVector2(Vector2 pos)
    {
        Vector2 _pos = Vector2.zero;
        _pos.x = Mathf.FloorToInt(pos.x / worldData.chunkSize);
        _pos.y = Mathf.FloorToInt(pos.y / worldData.chunkSize);
        return _pos;
    }

    void GenerateChunksAroundPlayer()
    {
        for (int x1 = (int)playerPosInChunks.x - worldData.viewDistanceInChunks; x1 < (int)playerPosInChunks.x + worldData.viewDistanceInChunks; x1++)
        {
            for (int y1 = (int)playerPosInChunks.y - worldData.viewDistanceInChunks; y1 < (int)playerPosInChunks.y + worldData.viewDistanceInChunks; y1++)
            {

                Vector2 cc = new Vector2(x1, y1);

                if (!FindChunkWithCoord(cc))
                {
                    GenerateChunk(cc, 0f);
                }
            }
        }
    }

    Chunk DestroyDistantChunk()
    {
        if (chunks.Count != 0) {

            for (int i = 0; i < chunks.Count - 1; i ++)
            {
                Vector2 _cc = chunks[i].chunkCoord;

                if (_cc.x > (int)playerPosInChunks.x + worldData.viewDistanceInChunks + 1 || _cc.x < (int)playerPosInChunks.x - (worldData.viewDistanceInChunks + 1) || _cc.y > (int)playerPosInChunks.y + worldData.viewDistanceInChunks + 1 || _cc.y < (int)playerPosInChunks.y - (worldData.viewDistanceInChunks + 1))
                {
                    DestroyChunk(chunks[i]);
                    return null;
                }

            }
        }

        return null;


    }

    void GenerateChunk(Vector2 chunkPos, float zLayer) {

        Chunk chunk = new Chunk();
        chunk.chunkCoord = chunkPos;
        chunk.parentObj = Instantiate(worldData.chunkParentSource, new Vector3(chunkPos.x, chunkPos.y, 0f), Quaternion.identity);
        chunk.parentObj.transform.SetParent(transform);

        // Structure Pass
        for (int x = 0; x < worldData.chunkSize; x++)
        {
            for (int y = 0; y < worldData.chunkSize; y++)
            {

                Vector3 tpip = new Vector3(x, y, zLayer);

                Vector3 _blockPos = new Vector3(chunkPos.x * worldData.chunkSize, chunkPos.y * worldData.chunkSize, 0f) + tpip;

                float _noiseBiomeValor = Noise.Get3DPerlin(_blockPos, worldData.worldSeed + 0.1f, worldData.biomeScale);
                int biomeID = GetBiomeType(_noiseBiomeValor);

                float _noiseValor = Noise.Get3DPerlin(_blockPos, worldData.worldSeed + worldData.biomes[biomeID].biomeSeed, worldData.biomes[biomeID].perlinScale);
                int blockID = GetBlockType(_noiseValor, biomeID, _blockPos);

                if (blockID != -1 && worldData.biomes[biomeID].blockTypes.Count - 1 >= blockID)
                {
                    AddBlock(_blockPos, chunk, worldData.biomes[biomeID].blockTypes[blockID]);
                }
            }
        }

        chunk.chunkID = chunks.Count;
        chunks.Add(chunk);
        chunksCoord.Add(chunkPos);
    }

    int GetBiomeType (float noiseValor)
    {
        if (noiseValor > 0.5f) { 
            return 0; 
        }

        return 1;
    }

    int GetBlockType(float noiseValor, int biomeType, Vector3 blockPos) {

        // 0 = Stone-like
        // 1 = Grass-like
        // 2 = Crystal
        // 3 = Mineral

        if (noiseValor > worldData.biomes[biomeType].perlinThreshold + 0.01f)
        {
            return 0;
        }
        else if (noiseValor > worldData.biomes[biomeType].perlinThreshold)
        {
            float grassLikeNoise = Noise.Get3DPerlin(blockPos, worldData.worldSeed, worldData.biomes[biomeType].perlinScale * 5f);

            if (grassLikeNoise > 0.5f)
                return 2;
            else
                return 1;
        }


        return -1;

    }

    public void AddBlock (Vector3 pos, Chunk c, int bType)
    {
        Block _blockBase = blocks[bType];
        GameObject block = Instantiate(_blockBase.blockObject, pos, Quaternion.identity);
        block.transform.SetParent(c.parentObj.transform);

        BlockData blockData = block.AddComponent<BlockData>();
        blockData.chunk = c;
        blockData.gO = block;
        blockData.blockType = _blockBase.blockName;
        blockData.blockBase = _blockBase;
        blockData.x = (int)Mathf.FloorToInt(pos.x);
        blockData.y = (int)Mathf.FloorToInt(pos.y);
        blockData.blockID = blockData.chunk.blocks.Count;
        blockData.blockTypeID = bType;

        c.blocks.Add(block);
    }

    public int DeleteBlock (GameObject block)
    {
        BlockData blockData = block.GetComponent<BlockData>();
        int _blockType = blockData.blockTypeID;
        blockData.chunk.blocks.RemoveAt(blockData.blockID);

        // actualise List
        if (blockData.chunk.blocks.Count >= blockData.blockID) {

            for (int i = blockData.blockID; i < blockData.chunk.blocks.Count; i++) {
                blockData.chunk.blocks[i].GetComponent<BlockData>().blockID = i;
            }

        }

        blockData.blockID = -1;
        Destroy(block.gameObject);
        return _blockType;

    }

    void DestroyChunk(Chunk _chunk)
    {
        int _chunkID = _chunk.chunkID;
        GameObject _chunkObj = _chunk.parentObj;
        chunksCoord.RemoveAt(_chunkID);
        chunks.RemoveAt(_chunkID);
        Destroy(_chunkObj);

        // recalculate chunksIDs
        for (int i = _chunkID; i < chunks.Count; i ++)
        {
            chunks[i].chunkID = i;
        }

    }

}