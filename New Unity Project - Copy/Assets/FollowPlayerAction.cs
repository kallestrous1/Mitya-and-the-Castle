using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Follow Player", story: "[Agent] follows [player]", category: "Action", id: "20bfe6ad87b7faedc5f1fbb26ad74022")]
public partial class FollowPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    [SerializeReference] public BlackboardVariable<float> Speed = new BlackboardVariable<float>(3.5f);

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
        MoveTowardsTarget(playerpos);
        return Status.Running;
    
    }

    private void MoveTowardsTarget(Vector2 targetPos)
    {
        Agent.Value.transform.position = Vector2.MoveTowards(Agent.Value.transform.position, targetPos, Speed * Time.deltaTime);
    }
}

