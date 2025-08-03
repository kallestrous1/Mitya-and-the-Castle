using System.Collections;
using UnityEngine;

public class ShopController : MonoBehaviour
{

    public InventoryObject currentShop;
    public DynamicInterface shopInterface;
    public GameObject shop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SetShop(currentShop));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetShop(InventoryObject newShop)
    {
        yield return new WaitForSeconds(1f);
        shopInterface.inventory = newShop;
        shopInterface.StartCreateSlots();
        newShop.Load();
    }

    public void setShopActive()
    {
        shop.SetActive(true);
    }

    public void setShopInactive()
    {
        shop.SetActive(false);
    }
}   
