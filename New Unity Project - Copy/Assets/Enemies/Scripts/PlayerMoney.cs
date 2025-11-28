using UnityEngine;

public class PlayerMoney :  DataPersistenceBehaviour
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

    public override void LoadData(GameData data)
    {
        playerMoney = data.playerMoney;
        PlayerMoneyDisplay.PlayerMoneyDisplayInstance.UpdateMoneyDisplay(playerMoney);
    }

    public override void SaveData(GameData data)
    {
        data.playerMoney = playerMoney;
    }

    public override void ResetData(GameData data)
    {
        playerMoney = 0;
    }
}
