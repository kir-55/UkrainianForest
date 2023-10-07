using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Workbench;

public class RecipeVisualiser : MonoBehaviour
{
    public CraftingMenu CraftingMenu;
    public Items[] Ingridients;
    public Items[] Products;
    [SerializeField] private GameObject ItemVisualisationPrefab;
    public ItemsInfo ItemsInfo;
    private void Start()
    {
        if (Ingridients != null && Ingridients.Length < 6)
        {
            foreach (Items Ingrigient in Ingridients)
            {
                GameObject ItemVisualisation = Instantiate(ItemVisualisationPrefab, transform.GetChild(0));
                ItemVisualisation.GetComponent<Image>().sprite = ItemsInfo.itemInfos[Ingrigient.tupe - 1].icon;
                ItemVisualisation.transform.GetChild(0).GetComponent<TMP_Text>().text = Ingrigient.amount.ToString();

            }
        }
        else
            Debug.Log("ERRORR");

        if (Products != null && Products.Length < 6)
        {
            foreach (Items Product in Products)
            {
                GameObject ItemVisualisation = Instantiate(ItemVisualisationPrefab, transform.GetChild(1));
                ItemVisualisation.GetComponent<Image>().sprite = ItemsInfo.itemInfos[Product.tupe - 1].icon;
                ItemVisualisation.transform.GetChild(0).GetComponent<TMP_Text>().text = Product.amount.ToString();

            }
        }
        else
            Debug.Log("ERRORR");
    }
    public void OnClick() 
    {
        if (Ingridients != null && Products != null)
            CraftingMenu.Craft(Ingridients, Products);
    } 
}
