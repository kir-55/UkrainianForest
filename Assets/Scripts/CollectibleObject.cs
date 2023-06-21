using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public int ItemTupe;
    public int itemAmount = 1;
    public float InstantiationTime;

    private void Start()
    {
        InstantiationTime = Time.time;
    

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
