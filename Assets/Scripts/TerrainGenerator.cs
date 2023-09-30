using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static TerrainGenerator;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap[] terrainTilemaps;
    [SerializeField] private Vector2Int mapSize;
    [SerializeField] private float scale = 10f;
    [SerializeField] private TerrainElement[] terrainElements;
    [SerializeField] private TerrainDecoration[] terrainDecorations;
    [SerializeField] private int chunkSize = 5;

    [SerializeField] private List<Vector2Int> loadedChunks;

    public float islandRadius = 30f; // Adjust the size of the island
    public float maxHeight = 1.0f;   // Adjust the maximum height of the terrain

    [Serializable]
    public class TerrainElement
    {
        public TileBase tile;
        public Tilemap tilemap;
        public float minHeight;
        public float maxHeight;
    }
    [Serializable]
    public class TerrainDecoration : TerrainElement
    {
        public float minOccurrenceLevel;
        public float maxOccurrenceLevel;
        public float scaleDecorations;
    }

    public float GetChunkStep() => terrainTilemaps[0].CellToWorld(new Vector3Int(chunkSize, 0)).x;

    private bool IsLoaded(Vector2Int chunkPos)
    {
        foreach(Vector2Int chunk in loadedChunks)
        {
            if(chunk == chunkPos)
                return true;
        }
        return false;
    }

    public void UnLoadChunks(Vector3Int pos, int radius = 25)
    {
        pos = terrainTilemaps[0].WorldToCell(pos);
        pos = new Vector3Int(pos.x - pos.x % chunkSize - 5, pos.y - pos.y % chunkSize - 5);
        List<Vector2Int> loadedChunks1 = loadedChunks;
            foreach (Vector2Int chunkPos in loadedChunks1.ToArray())
                if (Vector2.Distance(chunkPos, new Vector3(pos.x, pos.y)) >= radius)
                {
                    foreach (Tilemap terrainTilemap in terrainTilemaps)
                        for (int x = 0; x < chunkSize; x++)
                            for (int y = 0; y < chunkSize; y++)
                                terrainTilemap.SetTile(new Vector3Int(chunkPos.x + x, chunkPos.y + y), null);
                            
                        
                    loadedChunks.Remove(chunkPos);
                }
                    
        loadedChunks = loadedChunks1;
    }

    public void LoadChunk(Vector3Int chunkPos)
    {
        Vector2 center = new Vector2(mapSize.x / 2, mapSize.y /2);
        chunkPos = terrainTilemaps[0].WorldToCell(chunkPos);
        chunkPos = new Vector3Int(chunkPos.x - chunkPos.x % 5 - 3, chunkPos.y - chunkPos.y % 5 - 3);

        if (!IsLoaded(new Vector2Int(chunkPos.x, chunkPos.y)))
        {
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    Vector3Int position = new Vector3Int(chunkPos.x + x, chunkPos.y + y);

                    float xCoord = (1000 - (float)position.x) / mapSize.x * scale;
                    float yCoord = (1000 - (float)position.y) / mapSize.y * scale;
                    

                    float sample = Mathf.PerlinNoise(xCoord, yCoord);

                    foreach (TerrainElement currentElement in terrainElements)
                        if (sample <= currentElement.maxHeight && sample >= currentElement.minHeight)
                        {
                            currentElement.tilemap.SetTile(new Vector3Int(position.x, position.y), currentElement.tile);
                            break;
                        }
                        //else
                           // currentElement.tilemap.SetTile(new Vector3Int(position.x, position.y), terrainElements[0].tile);

                    foreach (TerrainDecoration currentDecoration in terrainDecorations)
                    {
                        float xCoordDec = (5000 - (float)position.x) / mapSize.x * currentDecoration.scaleDecorations;
                        float yCoordDec = (5000 - (float)position.y) / mapSize.y * currentDecoration.scaleDecorations;
                        float sampleDecorations = Mathf.PerlinNoise(xCoordDec, yCoordDec);

                        if (sample <= currentDecoration.maxHeight && sample >= currentDecoration.minHeight
                        && sampleDecorations >= currentDecoration.minOccurrenceLevel && sampleDecorations <= currentDecoration.maxOccurrenceLevel)
                        {
                            currentDecoration.tilemap.SetTile(new Vector3Int(position.x, position.y), currentDecoration.tile);
                            break;
                        }
                    }

                    
                    
                }
            }
            loadedChunks.Add(new Vector2Int(chunkPos.x, chunkPos.y));
        }
    }
}


