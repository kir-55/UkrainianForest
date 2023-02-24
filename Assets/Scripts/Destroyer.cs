using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField]private float timeToDestroy = 2;
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
