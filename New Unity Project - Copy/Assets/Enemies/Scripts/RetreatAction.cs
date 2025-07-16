using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Retreat", story: "Retreat away from [player] at [speed]", category: "Action", id: "5ccb235ee636901423f43f78ac9e8452")]
public partial class RetreatAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> Speed;
    [SerializeReference] public BlackboardVariable<string> Walk;

    [SerializeReference] public BlackboardVariable<Animator> Animator;

    private Rigidbody2D rb;
    private Enemy enemy;

    protected override Status OnStart()
    {
        enemy = Agent.Value.GetComponent<Enemy>();
        rb = Agent.Value.GetComponent<Rigidbody2D>();
        Animator.Value.SetBool(Walk.Value, true);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float distanceToPlayer = Vector2.Distance(Agent.Value.transform.position, Player.Value.transform.position);
        if (distanceToPlayer > 10f || distanceToPlayer <4f)
        {
            Animator.Value.SetBool(Walk.Value, false);
            return Status.Success;
        }
        float directionMultiplier = 1;
        if (enemy.facingRight)
        {
            directionMultiplier = -1;
        }
        rb.AddForce(new Vector2(Speed.Value * directionMultiplier, 0), ForceMode2D.Impulse);

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

