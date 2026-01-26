using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Detector sees player", story: "true if [detector] sees [player]", category: "Conditions", id: "7ac1619d5459972ea9d42b961ccf6905")]
public partial class DetectorSeesPlayerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<AlertAreaScript> Detector;
    [SerializeReference] public BlackboardVariable<GameObject> Player;

    public override bool IsTrue()
    {
        if(!Detector.Value)
        {
            Debug.LogWarning("Detector is not assigned in DetectorSeesPlayerCondition");
            return false;
        }

        if (Detector.Value.playerDetected)
        {
            Player.Value = Detector.Value.Player;
            return true;
        }
        return false;
    }

}
