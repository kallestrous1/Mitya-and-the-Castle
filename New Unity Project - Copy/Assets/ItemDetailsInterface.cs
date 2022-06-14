using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsInterface : MonoBehaviour
{

    public void setInterface(ItemObject item)
    {
        this.transform.Find("Description").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.description;
        this.transform.Find("Image").GetComponent<Image>().sprite = item.uiDisplay;
        this.transform.Find("Name").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.name;
    }

}
