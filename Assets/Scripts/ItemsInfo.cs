using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsInfo : MonoBehaviour
{

    [System.Serializable]
    public class ItemTupeInfos
    {
        public string name;
        public Sprite icon;
        public int typeNumber;
        public bool isBlock;
        public bool isPowerup;
    }

    // Array of collectible types
    public ItemTupeInfos[] itemTupesInfos;
}
