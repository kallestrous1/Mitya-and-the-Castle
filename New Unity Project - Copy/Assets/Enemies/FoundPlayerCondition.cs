using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Found Player", story: "[Detector] found [player]", category: "Conditions", id: "881d7c252be52a2ecac26d149d7968fe")]
public partial class FoundPlayerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<AlertAreaScript> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
