using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mosquito : Enemy
{
    [SerializeField] private Transform player;
    public AudioClip buzz;
    public AudioSource source;

    AlertAreaScript alertArea;
    bool holding;

    public override void Start()
    {
        base.Start();
        alertArea = GetComponentInChildren<AlertAreaScript>();
        if (source && buzz)
            source.clip = buzz;
    }

    public override void processHit()
    {
        ani.SetTrigger("Stun");
    }

    public override void Update()
    {
        base.Update();
    }

    void LateUpdate()
    {
        if (alertArea != null && alertArea.playerDetected)
        {
            holding = false;
            if (source && buzz && !source.isPlaying)
                source.Play();
            return;
        }

        if (holding)
            StopCombat();
    }

    public override void OnPlayerLeftRange()
    {
        holding = true;
        StopCombat();
    }

    public override bool IsPlayerInCombatRange()
    {
        return alertArea != null && alertArea.playerDetected;
    }

    void StopCombat()
    {
        if (source && source.isPlaying)
            source.Stop();
        if (rb)
            rb.linearVelocity = Vector2.zero;
        if (!ani)
            return;

        ani.ResetTrigger("Attack");
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("New State"))
            ani.Play("New State", 0, 0);
    }

    private void Leap()
    {
        if (alertArea == null || !alertArea.playerDetected || target == null)
            return;

        Vector2 targetPos = (Vector2)target.transform.position;
        Vector2 dir = targetPos - (Vector2)transform.position;
        dir.Normalize();
        transform.GetComponent<Rigidbody2D>().AddForce(dir * 500, ForceMode2D.Impulse);
    }
}
