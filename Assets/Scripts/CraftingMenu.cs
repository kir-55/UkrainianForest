using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Workbench;

public class CraftingMenu : MonoBehaviour
{
    private List<Recipe[]> activeWorkbenches;
    [SerializeField] private ItemsInfo itemsInfo;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject craftingMenuPanel;
    [SerializeField] private GameObject recipePanelPrefab;
    [SerializeField] private GameObject playerDropPoint;
    [SerializeField] public Recipe[] existingRecipes;
    private void Start()
    {
        activeWorkbenches = new List<Recipe[]>();
        AddWorkbench(existingRecipes);
    }
    public void AddWorkbench(Recipe[] recipes)
    {
        print("added");
        activeWorkbenches.Add(recipes);
        if (craftingMenuPanel.activeSelf)
            RwenerateRecipes();
    }
    public void RemoveWorkbench(Recipe[] recipes)
    {
        print("removed");
        activeWorkbenches.Remove(recipes);
        if (craftingMenuPanel.activeSelf)
            RwenerateRecipes();
    }
    private void RwenerateRecipes()
    {
        print("rewriteing");
        Transform content = craftingMenuPanel.transform.GetChild(0).GetChild(0);

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        for (int i = 0; i < activeWorkbenches.Count; i++)
        {
            for(int j = 0; j < activeWorkbenches[i].Length; j++)
            {
                RecipeVisualiser currentRecipeVisualiser = Instantiate(recipePanelPrefab, content).GetComponent<RecipeVisualiser>();
                currentRecipeVisualiser.ItemsInfo = itemsInfo;
                currentRecipeVisualiser.Products = activeWorkbenches[i][j].Products;
                currentRecipeVisualiser.Ingridients = activeWorkbenches[i][j].Ingridients;
                currentRecipeVisualiser.CraftingMenu = this;
            }
        }
    }
    
    public void SwithCraftingMenu()
    {
        if(craftingMenuPanel.activeSelf)
            craftingMenuPanel.SetActive(false);
        else 
        { 
            RwenerateRecipes();
            craftingMenuPanel.SetActive(true);
        }  
    }
    public bool Craft(Items[] ingridients, Items[] products) 
    {
        if (ingridients != null && products != null)
        {
            int i = 0;
            foreach (Items ingridient in ingridients)
            {
                if (inventory.GetItemAmount(ingridient.tupe) >= ingridient.amount)
                    i++;
            }
            
                
            
            if (i == ingridients.Length)
            {
                foreach (Items ingridient in ingridients)
                {
                    inventory.RemoveItem(ingridient.tupe, inventory.GetSlots(), true, ingridient.amount);
                    //inventory.RewriteInventory();
                }
                    

                foreach (Items product in products)
                    if (itemsInfo.itemInfos.Length > 0)
                    {
                        GameObject droppedProduct = Instantiate(itemsInfo.ItemSample, playerDropPoint.transform.position, Quaternion.identity);
                        droppedProduct.GetComponent<SpriteRenderer>().sprite = itemsInfo.itemInfos[product.tupe - 1].icon;
                        droppedProduct.GetComponent<CollectibleObject>().ItemTupe = product.tupe;
                        droppedProduct.GetComponent<CollectibleObject>().itemAmount = product.amount;
                    }
            }


        }
        return true;
    }
} 
