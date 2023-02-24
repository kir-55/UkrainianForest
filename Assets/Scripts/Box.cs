using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private int i;
    [SerializeField]private GameObject dack;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "bullet")
            i++;
        if(i == 3)
        {
            Destroy(gameObject);
            Instantiate(dack,transform.position,transform.rotation);
        }
            
    }
}
