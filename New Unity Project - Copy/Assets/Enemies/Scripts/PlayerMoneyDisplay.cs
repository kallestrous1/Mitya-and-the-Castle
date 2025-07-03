using UnityEngine;
using TMPro;


public class PlayerMoneyDisplay : MonoBehaviour
{
    public TextMeshProUGUI moneyDisplay;
    public static PlayerMoneyDisplay PlayerMoneyDisplayInstance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (PlayerMoneyDisplayInstance != null)
        {
            Debug.Log("creating another moneydisplayinstance (not good)");
        }
        PlayerMoneyDisplayInstance = this;
    }

    public void UpdateMoneyDisplay(int currentMoneyCount)
    {
        moneyDisplay.text = currentMoneyCount.ToString();
    }

}
