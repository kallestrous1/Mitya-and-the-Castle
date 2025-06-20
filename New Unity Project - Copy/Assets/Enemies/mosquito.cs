using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mosquito : Enemy
{
    [SerializeField] private Transform player;
    public AudioClip buzz;
    public AudioSource source;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        source.clip = buzz;
        source.Play();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void Leap()
    {
        Vector2 targetPos = (Vector2)target.transform.position;
        Vector2 dir = targetPos - (Vector2)transform.position;
        dir.Normalize();
        transform.GetComponent<Rigidbody2D>().AddForce(dir * 500, ForceMode2D.Impulse);
    }
}
