using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Agent action", story: "[Agent] ready for action", category: "Conditions", id: "735906c4d8645995ed9814824dbafe8f")]
public partial class AgentActionCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Enemy> Agent;

    public override bool IsTrue()
    {
        if (Agent.Value!=null)
        {
            return Agent.Value.checkIfReadyForAction();
        }
        else
        {
            return false;
        }
    }
}
