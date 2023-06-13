using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workbench;

public class CraftingMenu : MonoBehaviour
{
    private List<Workbench> activeWorkbenches;
    [SerializeField] private ItemsInfo itemsInfo;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject craftingMenuPanel;
    [SerializeField] private GameObject recipePanelPrefab;
    [SerializeField] private GameObject playerDropPoint;
    private void Start()
    {
        activeWorkbenches = new List<Workbench>();  
    }
    public void AddWorkbench(Workbench workbench)
    {
        print("added");
        activeWorkbenches.Add(workbench);
        if (craftingMenuPanel.activeSelf)
            RwenerateRecipes();
    }
    public void RemoveWorkbench(Workbench workbench)
    {
        print("removed");
        activeWorkbenches.Remove(workbench);
        if (craftingMenuPanel.activeSelf)
            RwenerateRecipes();
    }
    private void RwenerateRecipes()
    {
        print("rewriteing");
        Transform content = craftingMenuPanel.transform.GetChild(0).GetChild(0);

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i));

        for (int i = 0; i < activeWorkbenches.Count; i++)
        {
            for(int j = 0; j < activeWorkbenches[i].recipes.Length;)
            {
                RecipeVisualiser currentRecipeVisualiser = Instantiate(recipePanelPrefab, content).GetComponent<RecipeVisualiser>();
                currentRecipeVisualiser.ItemsInfo = itemsInfo;
                currentRecipeVisualiser.Products = activeWorkbenches[i].recipes[j].Products;
                currentRecipeVisualiser.Ingridients = activeWorkbenches[i].recipes[j].Ingridients;
                currentRecipeVisualiser.CraftingMenu = this;
            }
        }
    }
    
    public void ActivateCraftingMenu()
    {
        RwenerateRecipes();
        craftingMenuPanel.SetActive(true);
    }
    public void DeactivateCraftingMenu()
    {
        craftingMenuPanel.SetActive(false);    
    }
    public bool Craft(Items[] ingridients, Items[] products) 
    {
        if(ingridients != null && products != null) 
        {
            foreach (Items ingridient in ingridients)
                if (inventory.GetItemAmount(ingridient.tupe) >= ingridient.amount)
                    inventory.RemoveItem(ingridient.tupe, inventory.GetSlots(), true, ingridient.amount);

            foreach (Items product in products)
                if (itemsInfo.itemTupesInfos.Length > 0) 
                { 
                    GameObject droppedProduct = Instantiate(itemsInfo.itemTupesInfos[product.tupe].prefab, playerDropPoint.transform.position, Quaternion.identity);
                    droppedProduct.GetComponent<CollectibleObject>().itemAmount = product.amount;
                }
                                    
        }
        return true;
    }
}
