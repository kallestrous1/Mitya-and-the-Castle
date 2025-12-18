using UnityEngine;

public class InGameMoneyPickUp : Interactable
{
    [SerializeField] public int Value;
    public AudioClip pickUpMoneySound;
    public override void interact()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>().ChangePlayerMoneyCount(Value);
        AudioManager.Instance.Play(pickUpMoneySound);
        GetComponent<ItemInGame>().ChangeState(false);
        Destroy(this.transform.parent.gameObject);
    }

}
