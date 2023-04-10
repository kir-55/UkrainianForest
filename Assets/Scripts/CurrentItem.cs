using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentItem : MonoBehaviour
{
    [SerializeField] private ItemsInfo itemsInfo;
    ItemsInfo.ItemTupeInfos itemInfos;
    [SerializeField] private CameraController cameraController;
    private SpriteRenderer spriteRenderer;
    GameControler gameControler;

    private void Awake()
    {
        gameControler = new GameControler();
        gameControler.Gamepad.Put.performed += ctx => Put();
    }
    private void Start()
    {    
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Put()
    {
        if(itemInfos != null && itemInfos.isBlock)
            Debug.Log("puting");
    }

    public void SetCurrentItem(int itemTupe)
    {
        if(itemTupe > 0) 
        {
            try
            {
                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            }
            catch
            {

            }
            
            
            spriteRenderer.color = new Color(1, 1, 1, 1);
            itemInfos = itemsInfo.itemTupesInfos[itemTupe-1];
            if(itemInfos.iconInHand)
                spriteRenderer.sprite = itemInfos.iconInHand;
            else if(itemInfos.icon)
                spriteRenderer.sprite = itemInfos.icon;
            if(itemInfos.canBeUsed)
            {
                if(itemInfos.prefabInHand)
                    Instantiate(itemInfos.prefabInHand, transform.GetChild(0).position, transform.GetChild(0).rotation, transform.GetChild(0)); 
            }
        }
        else 
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            try
            {
                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            }
            catch
            {

            }
        }
    }
    public CameraController GetCameraController() => cameraController;
    void OnEnable()
    {
        gameControler.Gamepad.Enable();
    }

    void OnDisable()
    {
        gameControler.Gamepad.Disable();
    }
}
