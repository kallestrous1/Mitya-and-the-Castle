using UnityEngine;

public class InGameMoneyPickUp : Interactable
{
    [SerializeField] public int Value;
    public AudioClip pickUpMoneySound;
    public override void interact()
    {
        TelemetryManager.instance.LogEvent("Money", "Picked up money worth " + Value);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>().ChangePlayerMoneyCount(Value);
        AudioManager.Instance.Play(pickUpMoneySound);
        GetComponentInParent<StateChangingObject>().ChangeObjectState(false);
    }

}
