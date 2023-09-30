using Unity.Mathematics;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    [SerializeField] private int chunkSize;
    [SerializeField] private TerrainGenerator generator;
    [SerializeField] private Transform playerTransform;
    private Vector3Int playerPosRecent, playerPos;
    private float chunkStep;

    private void Start()
    {
        chunkStep = generator.GetChunkStep();
        LoadChunks();
        //playerPosRecent = Vector3Int.zero;
    }

    private void Update()
    {
        playerPos = new Vector3Int((int)playerTransform.position.x, (int)playerTransform.position.y);
        if(playerPos != playerPosRecent)
        {

            LoadChunks();
            playerPosRecent = playerPos;
        }
    }

    private void LoadChunks()
    {
        generator.LoadChunk(new Vector3Int(playerPos.x, playerPos.y));
        generator.LoadChunk(new Vector3Int(playerPos.x + (int)chunkStep, playerPos.y));
        generator.LoadChunk(new Vector3Int(playerPos.x - (int)chunkStep, playerPos.y));
        generator.LoadChunk(new Vector3Int(playerPos.x, playerPos.y - (int)chunkStep));
        generator.LoadChunk(new Vector3Int(playerPos.x, playerPos.y + (int)chunkStep));

        generator.LoadChunk(new Vector3Int(playerPos.x + (int)chunkStep, playerPos.y + (int)chunkStep));
        generator.LoadChunk(new Vector3Int(playerPos.x - (int)chunkStep, playerPos.y - (int)chunkStep));
        generator.LoadChunk(new Vector3Int(playerPos.x + (int)chunkStep, playerPos.y - (int)chunkStep));
        generator.LoadChunk(new Vector3Int(playerPos.x - (int)chunkStep, playerPos.y + (int)chunkStep));

        generator.UnLoadChunks(playerPos);
    }
}


