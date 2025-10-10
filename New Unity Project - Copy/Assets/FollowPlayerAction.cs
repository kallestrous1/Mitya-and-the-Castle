using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Follow Player", story: "[Agent] follows [player] at [speed]", category: "Action", id: "20bfe6ad87b7faedc5f1fbb26ad74022")]
public partial class FollowPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> Speed;

    private float stoppingDistance = 10;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("no agent assigned");
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 playerpos = Player.Value.transform.position;
        return(MoveTowardsTarget(playerpos));
    
    }

    private Status MoveTowardsTarget(Vector2 targetPos)
    {
        Vector2 pos = (Vector2)Agent.Value.transform.position;

        float distance = Vector2.Distance(pos, targetPos);
        if (distance > stoppingDistance)
        {
            Vector2 dir = (targetPos - pos).normalized;
            Agent.Value.GetComponent<Rigidbody2D>().AddForce(dir * Speed.Value, ForceMode2D.Impulse);
        }
        else
        {
            return Status.Success;
        }
        return Status.Running;
    }
}

