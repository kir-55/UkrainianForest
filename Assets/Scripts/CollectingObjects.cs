using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingObjects : MonoBehaviour
{
    [SerializeField] private AudioClip collectingSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.GetComponent<CollectibleObject>() && collider2D.gameObject.GetComponent<CollectibleObject>().isCollectable)
        {
            CollectibleObject targetObject = collider2D.gameObject.GetComponent<CollectibleObject>();
            audioSource.PlayOneShot(collectingSound);
            audioSource.transform.position = transform.position;
            audioSource.transform.rotation = transform.rotation;
            targetObject.Collect();

        }
    }
}
