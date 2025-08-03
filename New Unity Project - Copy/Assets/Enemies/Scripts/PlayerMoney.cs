using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public float playerMoney = 5;

    private void Start()
    {
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }

    public void ChangePlayerMoneyCount(float change)
    {
        playerMoney += change;
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }
}
