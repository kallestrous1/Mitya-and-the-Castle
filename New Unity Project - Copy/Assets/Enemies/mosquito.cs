using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mosquito : Enemy
{
    [SerializeField] private Transform player;



    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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
        transform.position = Vector2.MoveTowards(transform.position, targetPos, 10 * Time.deltaTime);
    }
}
