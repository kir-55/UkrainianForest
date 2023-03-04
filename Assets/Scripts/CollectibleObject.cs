using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    // Define a class to hold information about each collectible type
    public bool isCollectable;


    [System.Serializable]
    public class CollectibleType
    {
        public string name;
        public Sprite icon;
        public int typeNumber;
        public bool isBlock;
        public bool isPowerup;
    }

    // Array of collectible types
    public CollectibleType collectibleType;

    

    // Method to collect the object and apply its effect
    public void Collect()
    {
        if(isCollectable)
        {
            
            // animation of disappear
            // ||||||||||||||||||||||
            Destroy(gameObject);
        }     
    }
}
