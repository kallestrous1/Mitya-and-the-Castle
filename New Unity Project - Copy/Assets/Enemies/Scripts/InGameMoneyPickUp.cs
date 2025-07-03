using UnityEngine;

public class InGameMoneyPickUp : Interactable
{
    [SerializeField] public int Value;

    public override void interact()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoney>().ChangePlayerMoneyCount(Value);
        Destroy(this.transform.parent.gameObject);
    }

}
