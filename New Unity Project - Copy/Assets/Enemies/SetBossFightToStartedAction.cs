using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set BossFight to Started", story: "set [bossfight] to started", category: "Action", id: "a7bef290cb9c5f1f259322fd179463de")]
public partial class SetBossFightToStartedAction : Action
{
    [SerializeReference] public BlackboardVariable<Boss> Bossfight;

    protected override Status OnStart()
    {
        Bossfight.Value.StartFight();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

