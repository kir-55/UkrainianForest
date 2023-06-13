using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public CraftingMenu craftingMenu;
    [System.Serializable]
    public class Items
    {
        public int tupe;
        public int amount = 1;
    }
    [System.Serializable]
    public class Recipe
    {
        [SerializeField] public Items[] Ingridients;
        [SerializeField] public Items[] Products;
    }

    [SerializeField] public Recipe[] recipes;

    private void OnTriggerEnter2D(Collider2D collider)
    {
       if (collider.gameObject.CompareTag("player"))
       {
            if (craftingMenu == null)
                craftingMenu = collider.gameObject.GetComponent<Inventory>().GetCraftingMenu();

            craftingMenu.AddWorkbench(recipes);
       }
            
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("player") && craftingMenu)
            craftingMenu.RemoveWorkbench(recipes);
    }
}
