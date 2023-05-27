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
            if (other.TryGetComponent(out collectible) && collectible.IsCollectable)
            {
                if (playerInventory.AddItem(collectible.ItemTupe, playerInventory.GetSlots(),playerInventory.MaxSlotsInRightHand))
                {
                    collectible.Collect();
                    audioSource.PlayOneShot(collectingSound);
                    audioSource.transform.position = playerTransform.position;
                    audioSource.transform.rotation = playerTransform.rotation;
                }
            }
        }
        print("collecting (36)");
    }
}
