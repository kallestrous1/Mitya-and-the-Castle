using System.Collections;
using UnityEngine;

public class ShopController : MonoBehaviour
{

    public InventoryObject currentShop;
    public DynamicInterface shopInterface;
    public GameObject shop;

 

    public void SetShop(InventoryObject newShop)
    {
        StartCoroutine(SetShopCoroutine(newShop));
    }

    public IEnumerator SetShopCoroutine(InventoryObject newShop)
    {       
        shopInterface.inventory = newShop;
        shopInterface.StartCreateSlots();
        newShop.Load();
        foreach (var slot in newShop.GetSlots)
        {
           if(slot.item != null)
           {
               slot.UpdateSlot(slot.item, true);
            }
        }
        Debug.Log("setting shop to new shop: " + newShop.name);
        currentShop = newShop;
        yield return new WaitForSeconds(1f);
    }

    public void SetShopActive()
    {
        if (currentShop == null || shop == null)
        {
            Debug.Log("No current shop to set active.");
            return;
        }
        Debug.Log(currentShop);
        currentShop.Load();
        shop.SetActive(true);
    }

    public void SetShopInactive()
    {
        if(currentShop == null || shop == null)
        {
            Debug.Log("No current shop to set inactive.");
            return;
        }
        shop.SetActive(false);
        currentShop.Save();
    }
}   
