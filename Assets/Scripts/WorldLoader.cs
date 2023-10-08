using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldLoader : MonoBehaviour
{
    [SerializeField] public TextAsset worldFile;
    private string path;
    public ChangedBlocksList changedBlocksList;
    [SerializeField] private TileBase[] tiles;

    [System.Serializable]
    public class ChangedBlocksData
    {
        public int block;
        public int tileMap;
        public int posX;
        public int posY;
    }
    [System.Serializable]
    public class ChangedBlocksList
    {
        public List<ChangedBlocksData> changedBlocksData;
    }
    public void OutputJSON()
    {
        string output = JsonUtility.ToJson(changedBlocksList);
        File.WriteAllText(path, output);
    }

    public void RefreshJson()
    {
        path = Application.dataPath + "/Resources/changedBlocksData.json";
        Debug.Log(path);
        changedBlocksList = JsonUtility.FromJson<ChangedBlocksList>(worldFile.text);
    }
    //runtime things
    private void Start()
    {
        worldFile = Resources.Load<TextAsset>("changedBlocksData");
        RefreshJson();
        //ChangeBlock(11, 2, 4, 0);
    }

    public TileBase IsBlock(int x, int y, int tilemap)
    {
        for (int i = 0; i < changedBlocksList.changedBlocksData.Count; i++)
        {
            ChangedBlocksData existingBlock = changedBlocksList.changedBlocksData[i];
            if (existingBlock.posX == x && existingBlock.posY == y && existingBlock.tileMap == tilemap)
                return tiles[existingBlock.block];

        }
        return null;
    }

    public void ChangeBlock(TileBase block, int tilemap, int x, int y)
    {
        ChangedBlocksData newBlock = new ChangedBlocksData();
        for (int i = 0; i < tiles.Length; i++)
            if (tiles[i] == block)
                newBlock.block = i;
        
        newBlock.tileMap = tilemap;
        newBlock.posX = x;
        newBlock.posY = y;

        for (int i = 0; i < changedBlocksList.changedBlocksData.Count; i++)
        {
            ChangedBlocksData existingBlock = changedBlocksList.changedBlocksData[i];
            if (existingBlock.tileMap == newBlock.tileMap && existingBlock.posX == newBlock.posX && existingBlock.posY == newBlock.posY)
            {
                changedBlocksList.changedBlocksData[i].block = newBlock.block;
                return;
            }  
        }

        changedBlocksList.changedBlocksData.Add(newBlock);
    }
    
    void OnApplicationQuit()
    {
        OutputJSON();
    }
}