using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Health over x", story: "check [agent] [health] over [healthLost]", category: "Conditions", id: "5e40a4088c1d04508b31f78f2373f9e7")]
public partial class HealthOverHalfCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Destructable> Health;

    [SerializeReference] public BlackboardVariable<float> healthLost;


    public override bool IsTrue()
    {
        if (Health.Value.currentHealth > Health.Value.maxHealth - healthLost.Value)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
