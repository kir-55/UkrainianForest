using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CurrentItem : MonoBehaviour
{
    [SerializeField] private ItemsInfo itemsInfo;
    ItemsInfo.ItemTupeInfos itemInfos;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SpriteRenderer leftHandSpriteRenderer;
    [SerializeField] private SpriteRenderer rightHandSpriteRenderer;
    [SerializeField] private GameObject leftHandGunPoint, rightHandGunPoint, dropPoint;
    [SerializeField] private Tilemap blockTileMap, tileMap;
    [SerializeField] private Inventory inventory;
    public BlockInfos[] blocksInfos;
    private int rightHandItemTupe = -1, leftHandItemTupe = -1;
    private GameControler gameControler;

    [System.Serializable]
    public class BlockInfos
    {
        public TileBase Tile;
        public GameObject DropCollectableObject;
        public int DropCollectableObjectAmount;
    }

    private void Awake()
    {
        gameControler = new GameControler();
        gameControler.Gamepad.Put.performed += ctx => Put();
        gameControler.Gamepad.BreakDown.performed += ctx => BreakDown();
    }
    private void Put()
    {
        if(rightHandItemTupe > -1)
        {
            itemInfos = itemsInfo.itemTupesInfos[rightHandItemTupe - 1];
            Vector3Int tilePosition = tileMap.WorldToCell(dropPoint.transform.position);
        
            if (itemInfos != null && itemInfos.tile && blockTileMap.GetTile(tilePosition) == null)
            {
                (itemInfos.isBlock ? blockTileMap : tileMap).SetTile(tilePosition, itemInfos.tile);
                inventory.RemoveItem(rightHandItemTupe, inventory.GetSlots());
            }
        }
    }
    private void BreakDown()
    {
        Vector3 blockPosition = dropPoint.transform.position;
        Vector3Int tilePosition = tileMap.WorldToCell(blockPosition);
        TileBase tile = (blockTileMap.GetTile(tilePosition) ? blockTileMap.GetTile(tilePosition) : tileMap.GetTile(tilePosition));
        (blockTileMap.GetTile(tilePosition) ? blockTileMap : tileMap).SetTile(tilePosition, null);
        foreach (BlockInfos block in blocksInfos)
        {
            if (block.Tile == tile)
            {
                GameObject DroppedItem = Instantiate(block.DropCollectableObject, blockPosition, Quaternion.identity);
                DroppedItem.GetComponent<CollectibleObject>().itemAmount = block.DropCollectableObjectAmount;
            }
        }
    }

    public void SetCurrentItem(int itemTupe, bool isRightHand = true)
    {
        SpriteRenderer spriteRenderer = (isRightHand ? rightHandSpriteRenderer : leftHandSpriteRenderer); 
        GameObject gunPoint = (isRightHand ? rightHandGunPoint : leftHandGunPoint);

        if (isRightHand)
            rightHandItemTupe = itemTupe;
        else
            leftHandItemTupe = itemTupe;

        if (gunPoint.transform.childCount > 0) 
            Destroy(gunPoint.transform.GetChild(0).gameObject);

        if (itemTupe > 0) 
        {
            itemInfos = itemsInfo.itemTupesInfos[itemTupe-1];
            if(itemInfos.iconInHand || (itemInfos.icon && !itemInfos.canBeUsed))
            {
                if(itemInfos.iconInHand)
                    spriteRenderer.sprite = itemInfos.iconInHand;
                else
                    spriteRenderer.sprite = itemInfos.icon;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            else
                spriteRenderer.color = new Color(1, 1, 1, 0);
            if (itemInfos.canBeUsed && itemInfos.prefabInHand)
                Instantiate(itemInfos.prefabInHand, gunPoint.transform.position, gunPoint.transform.rotation, gunPoint.transform); 

        }
        else 
            spriteRenderer.color = new Color(1, 1, 1, 0);

    }
    
    public CameraController GetCameraController() => cameraController;
    void OnEnable()
    {
        gameControler.Gamepad.Enable();
    }

    void OnDisable()
    {
        gameControler.Gamepad.Disable();
    }
}
