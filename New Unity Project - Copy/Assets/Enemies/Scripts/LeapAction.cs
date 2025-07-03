using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Leap", story: "[Agent] leaps at [player]", category: "Action", id: "8499cee201796d71771c3970affc0d70")]
public partial class LeapAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Animator> ani;


    protected override Status OnStart()
    {
        if(Agent.Value == null || ani.Value == null)
        {
            return Status.Failure;
        }
        ani.Value.SetTrigger("Attack");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Running;
    }

}

