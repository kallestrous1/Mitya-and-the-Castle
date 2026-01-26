using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player In Range", story: "[Player] in [range]", category: "Conditions", id: "c63471827ce13c2c4e6f154ea94af261")]
public partial class PlayerInRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<PlayerInRangeDetector> Detector;

    public override bool IsTrue()
    {
        if (Detector.Value.playerInRange)
        {
            Player.Value = Detector.Value.Player;
            return true;
        }
        return false;
    }

}
