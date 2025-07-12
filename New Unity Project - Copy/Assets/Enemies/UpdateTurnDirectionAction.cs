using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "update turn direction", story: "update turn direction from [script]", category: "Action", id: "cd29f0d22fbac8ffb21ce1dd63a3991a")]
public partial class UpdateTurnDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Script;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Script.Value.UpdateTurnDirection();
        return Status.Running;
    }

    protected override void OnEnd()
    {
        
    }
}

