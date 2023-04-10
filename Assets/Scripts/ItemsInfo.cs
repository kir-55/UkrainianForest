using System;
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
        public Sprite iconInHand;
        public GameObject prefab;
        public int typeNumber;
        public GameObject prefabInHand;
        public bool isBlock;
        public bool isPowerup;
        public bool canBeUsed;
    }

    // Array of collectible types
    public ItemTupeInfos[] itemTupesInfos;
}
