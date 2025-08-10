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
        Debug.Log("setting shop to new shop: " + newShop.name);
        currentShop = newShop;
        yield return new WaitForSeconds(1f);
    }

    public void setShopActive()
    {
        Debug.Log(currentShop);
        currentShop.Load();
        shop.SetActive(true);
    }

    public void setShopInactive()
    {
        shop.SetActive(false);
        currentShop.Save();
    }
}   
