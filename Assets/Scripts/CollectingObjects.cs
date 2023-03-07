using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingObjects : MonoBehaviour
{
    [SerializeField] private AudioClip collectingSound;
    [SerializeField] private LayerMask collectibleLayerMask;
    [SerializeField] private Inventory playerInventory;

    private AudioSource audioSource;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerTransform = transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & collectibleLayerMask) != 0)
        {
            CollectibleObject collectible = null;
            if (other.TryGetComponent(out collectible) && collectible.isCollectable)
            {
                // Add the collected item to the player's inventory
                if (playerInventory.AddItem(collectible.collectibleType.typeNumber))
                {
                    audioSource.PlayOneShot(collectingSound);
                    audioSource.transform.position = playerTransform.position;
                    audioSource.transform.rotation = playerTransform.rotation;
                    collectible.Collect();
                }
                else
                    Debug.Log("A new item cannot be added to the inventory because it is full!");
            }
            else
                Debug.Log("EEEEEERRROOREEE");
        }
        else
            Debug.Log("other.gameObject.layer: "+ other.gameObject.layer + " collectibleLayerMask: " + collectibleLayerMask.value);
    }
}
