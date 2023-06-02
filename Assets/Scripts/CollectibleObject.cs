using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    // Define a class to hold information about each collectible type
    public bool IsCollectable = true;
    public int ItemTupe;
    public int itemAmount = 1;
    public float InstantiationTime;

    // Method to collect the object and apply its effect
    private void Start()
    {
        InstantiationTime = Time.time;
    }
    public void Collect()
    {
        if(IsCollectable)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<CollectibleObject>())
        {
            CollectibleObject droppedItem = other.gameObject.GetComponent<CollectibleObject>();
            if (droppedItem.ItemTupe == ItemTupe)
            {
                itemAmount += droppedItem.itemAmount;

                if (InstantiationTime > droppedItem.InstantiationTime)
                    Destroy(other.gameObject);
                  
            }
        }
    }
}
