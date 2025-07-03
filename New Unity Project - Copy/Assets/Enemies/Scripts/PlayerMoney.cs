using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    int playerMoney = 5;

    private void Start()
    {
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }

    public void ChangePlayerMoneyCount(int change)
    {
        playerMoney += change;
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }
}
