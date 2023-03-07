using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    // Define a class to hold information about each collectible type
    public bool IsCollectable = true;
    public int ItemTupe;

    // Method to collect the object and apply its effect
    public void Collect()
    {
        if(IsCollectable)
            Destroy(gameObject);
    }
}
