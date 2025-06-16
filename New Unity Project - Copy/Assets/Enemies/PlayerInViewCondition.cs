using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player in view", story: "check if [detector] sees [player]", category: "Conditions", id: "0f7710ff4abe65d39cbcfff070d23811")]
public partial class PlayerInViewCondition : Condition
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
