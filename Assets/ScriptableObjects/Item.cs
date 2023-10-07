using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string title;
    public Sprite icon;
    public Sprite iconInHand;
    public int typeNumber;
    public GameObject prefabInHand;
    public TileBase tile;
    public GameObject blockPrefab;
    public bool isBlock;
    public bool isPowerup;
    public bool canBeUsed;
}
