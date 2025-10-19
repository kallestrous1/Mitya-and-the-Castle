using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsInterface : MonoBehaviour
{

    public void setInterface(ItemObject item)
    {
        this.transform.Find("Description").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.description;
        Image image = this.transform.Find("Image").GetComponent<Image>();
        image.sprite = item.uiDisplay;
        image.color = new Color(1, 1, 1, 1);
        this.transform.Find("Name").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.name;
    }

    public void ResetInterface()
    {
        this.transform.Find("Description").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        Image image = this.transform.Find("Image").GetComponent<Image>();
        image.sprite = null;
       image.color = new Color(1, 1, 1, 0);
        this.transform.Find("Name").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
    }

}
