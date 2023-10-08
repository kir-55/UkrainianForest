using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class CurrentItem : MonoBehaviour
{
    [SerializeField] private ItemsInfo itemsInfo;
    [SerializeField] private NavMeshUpdater navMeshUpdater;
    Item itemInfos;
    [SerializeField] private CraftingMenu craftingMenu;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SpriteRenderer leftHandSpriteRenderer;
    [SerializeField] private SpriteRenderer rightHandSpriteRenderer;
    [SerializeField] private GameObject leftHandGunPoint, rightHandGunPoint, dropPoint;
    [SerializeField] private Tilemap blockTileMap, tileMap;
    [SerializeField] private Inventory inventory;
    [SerializeField] private LayerMask blockObjectsLayer;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private Tile[] unbreakableTiles;
    [SerializeField] private GameObject[] unbreakableBlocks;
    [SerializeField] private WorldLoader worldLoader;

    public BlockInfos[] blocksInfos;
    
    private int rightHandItemTupe = -1, leftHandItemTupe = -1;
    private GameControler gameControler;

    [System.Serializable] 
    public class BlockInfos
    {
        public TileBase Tile;
        public GameObject BlockObject;
        public Item Item;
        public int Amount;
    }

    private void Awake()
    {
        gameControler = new GameControler();
        gameControler.Gamepad.Put.performed += ctx => Put();
        gameControler.Gamepad.BreakDown.performed += ctx => BreakDown();
    }
    private bool Put()
    {
        if(rightHandItemTupe > -1)
        {
            itemInfos = itemsInfo.itemInfos[rightHandItemTupe - 1];
            Vector3 blockPosition = dropPoint.transform.position;
            Vector3Int tilePosition = tileMap.WorldToCell(blockPosition);
            Collider2D blockColider = Physics2D.OverlapCircle(blockPosition, 0.1f, blockObjectsLayer);
            if (itemInfos != null  && blockTileMap.GetTile(tilePosition) == null && !blockColider)
            {
                if(itemInfos.tile || itemInfos.blockPrefab)
                {
                    if (itemInfos.blockPrefab)
                        Instantiate(itemInfos.blockPrefab, dropPoint.transform.position, Quaternion.identity);
                    else if(itemInfos.tile && (itemInfos.isBlock ? blockTileMap : tileMap).GetTile(tilePosition) != itemInfos.tile) 
                    {
                        (itemInfos.isBlock ? blockTileMap : tileMap).SetTile(tilePosition, itemInfos.tile);
                        worldLoader.ChangeBlock(itemInfos.tile, (itemInfos.isBlock ? 2 : 1), tilePosition.x, tilePosition.y);
                    } 
                    else
                        return false;
                    inventory.RemoveItem(rightHandItemTupe, inventory.GetSlots());
                    
                }
            }
        }
        return true;
    }
    private void BreakDown()
    {
        Vector3 blockPosition = dropPoint.transform.position;
        Vector3Int tilePosition = tileMap.WorldToCell(blockPosition);
        TileBase blockTile = blockTileMap.GetTile(tilePosition);
        TileBase tile = tileMap.GetTile(tilePosition);
        Collider2D blockColider = Physics2D.OverlapCircle(blockPosition, 0.1f, blockObjectsLayer);
        GameObject blockObject = (blockColider ? blockColider.gameObject : null);
        TileBase blockTileToFind = null;
        TileBase tileToFind = null;
        //GameObject blockObjectToFind = null;
        if (blockTileMap.GetTile(tilePosition))
        {
            blockTileToFind = blockTileMap.GetTile(tilePosition);
            foreach (Tile unbreakableTile in unbreakableTiles)
                if (unbreakableTile != null && unbreakableTile == blockTileToFind)
                    return;
            blockTileToFind = blockTileMap.GetTile(tilePosition);
            blockTileMap.SetTile(tilePosition, null);
            worldLoader.ChangeBlock(null, 2, tilePosition.x, tilePosition.y);
        }
        else if(blockObject)
        {
            foreach (GameObject unbreakableBlock in unbreakableBlocks)
                if (unbreakableBlock != null && unbreakableBlock == blockObject)
                    return;
            Destroy(blockObject);
            
        }
        else if (tileMap.GetTile(tilePosition))
        {
            tileToFind = tileMap.GetTile(tilePosition);
            tileMap.SetTile(tilePosition, null);
            worldLoader.ChangeBlock(null, 1, tilePosition.x, tilePosition.y);
        }  

        foreach (BlockInfos block in blocksInfos)
        {
            if ((blockTileToFind && block.Tile == blockTileToFind) || (blockObject && block.BlockObject && blockObject.name == string.Format("{0}(Clone)", block.BlockObject.name)) || (tileToFind && block.Tile == tileToFind))
            {
                GameObject DroppedItem = Instantiate(itemsInfo.ItemSample, blockPosition, Quaternion.identity);
                DroppedItem.GetComponent<SpriteRenderer>().sprite = block.Item.icon;
                DroppedItem.GetComponent<CollectibleObject>().ItemTupe = block.Item.typeNumber ;
                DroppedItem.GetComponent<CollectibleObject>().itemAmount = block.Amount;
            }
        }

        //Invoke("UpdateMesh", 0.1f);
        navMeshUpdater.changes += 100;
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
            itemInfos = itemsInfo.itemInfos[itemTupe-1];
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
