using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Bossfight started", story: "BossFight [Started]", category: "Conditions", id: "e91563c101f61824a4b3f45d11ca1187")]
public partial class BossfightStartedCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> Started;

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
