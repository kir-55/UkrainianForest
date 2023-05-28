using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentItem : MonoBehaviour
{
    [SerializeField] private ItemsInfo itemsInfo;
    ItemsInfo.ItemTupeInfos itemInfos;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SpriteRenderer leftHandSpriteRenderer;
    [SerializeField] private SpriteRenderer rightHandSpriteRenderer;
    [SerializeField] private GameObject leftHandGunPoint, rightHandGunPoint;    
    private GameControler gameControler;

    private void Awake()
    {
        gameControler = new GameControler();
        gameControler.Gamepad.Put.performed += ctx => Put();
    }
    private void Put()
    {
        if(itemInfos != null && itemInfos.isBlock)
            Debug.Log("puting");
    }

    public void SetCurrentItem(int itemTupe, bool isRightHand = true)
    {
        SpriteRenderer spriteRenderer = (isRightHand ? rightHandSpriteRenderer : leftHandSpriteRenderer); 
        GameObject gunPoint = (isRightHand ? rightHandGunPoint : leftHandGunPoint);

        if (gunPoint.transform.childCount > 0) 
            Destroy(gunPoint.transform.GetChild(0).gameObject);

        if (itemTupe > 0) 
        {
            itemInfos = itemsInfo.itemTupesInfos[itemTupe-1];
            if(itemInfos.iconInHand || (itemInfos.icon && !itemInfos.canBeUsed))
            {
                if(itemInfos.iconInHand)
                    spriteRenderer.sprite = itemInfos.iconInHand;
                else
                    spriteRenderer.sprite = itemInfos.icon;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            if(itemInfos.canBeUsed && itemInfos.prefabInHand)
                Instantiate(itemInfos.prefabInHand, gunPoint.transform.position, gunPoint.transform.rotation, gunPoint.transform); 

        }
        else 
            spriteRenderer.color = new Color(1, 1, 1, 0);

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
