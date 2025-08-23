using UnityEngine;

public class PlayerMoney : MonoBehaviour, IDataPersistence
{
    public float playerMoney = 0;

    private void Start()
    {
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }

    public void ChangePlayerMoneyCount(float change)
    {
        playerMoney += change;
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }

    public void LoadData(GameData data)
    {
        playerMoney = data.playerMoney;
    }

    public void SaveData(GameData data)
    {
        data.playerMoney = playerMoney;
    }
}
