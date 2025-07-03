using UnityEngine;

public class Ant : Enemy
{
    public override void processHit()
    {
        ani.SetTrigger("Stun");
    }
}

