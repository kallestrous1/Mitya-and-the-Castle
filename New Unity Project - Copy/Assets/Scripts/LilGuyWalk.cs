using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilGuyWalk : StateMachineBehaviour
{
    public float speed = 2.0f;

    Transform player;
    Rigidbody2D rb;

    LookAtPlayer lookAtPlayer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        lookAtPlayer = animator.GetComponent<LookAtPlayer>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        Vector2 target = new Vector2(player.position.x, rb.position.y);
       // var knockBackDirection = player.transform.position - rb.transform.position;
       // Mathf.Sign(knockBackDirection.x);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        //rb.velocity = new Vector2(speed* (Mathf.Sign(knockBackDirection.x)), 0);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
    }

}
