using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ckech : MonoBehaviour
{
    [SerializeField]private GameObject hoseButton;
    [SerializeField]private GameObject outsideButton;
    [SerializeField]private GameObject ckechPoint;
    [SerializeField]private GameObject ckechObject;
    private bool ckech1;
    private void Start()
    {
        if(hoseButton != null)
        hoseButton.gameObject.SetActive(false);
        if(outsideButton != null)
        outsideButton.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(ckech1)
        {
            ckechObject.transform.position = ckechPoint.transform.position;
            ckechObject.transform.rotation = ckechPoint.transform.rotation;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "washing machine"||collision.gameObject.tag == "box"||collision.gameObject.tag == "goose")
        {
            // if(Input.GetMouseButtonDown(1))
            // {
            //     ckech1 = !ckech1;
            //     ckechObject = collision.gameObject;
            //     if(ckech1)
            //     {
            //         if(collision.gameObject.tag == "washing machine")
            //             PlayerPrefs.SetInt("chech",1);
            //         else if(collision.gameObject.tag == "box")
            //             PlayerPrefs.SetInt("chech",2);
            //         else 
            //             PlayerPrefs.SetInt("chech",3);
            //     }
            //     else
            //         PlayerPrefs.SetInt("chech",0);   
            // }
        }
        if(collision.gameObject.tag == "hose")
        {
            hoseButton.gameObject.SetActive(true);
        }
        if(collision.gameObject.tag == "outside")
        {
            outsideButton.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "hose")
        {
            hoseButton.gameObject.SetActive(false);
        } 
        if(collision.gameObject.tag == "outside")
        {
            outsideButton.gameObject.SetActive(false);
        } 
    }
}
