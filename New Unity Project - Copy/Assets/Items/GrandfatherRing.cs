using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "Golden Ring", menuName = "Inventory/Charms/Golden Ring")]

public class GrandfatherRing : ItemObject
{
    private string hasRing= "hasRing";
    Story grandfatherStory;
    private void Awake()
    {
    }

    public override void EquipItem()
    {
        grandfatherStory = new Story(itemStory.text);
        Debug.Log(grandfatherStory);
        Debug.Log(grandfatherStory.variablesState[hasRing]);
        grandfatherStory.variablesState[hasRing] = true;
        Debug.Log(grandfatherStory.variablesState[hasRing]);
        DialogueManager.instance.dialogueVariables.ChangeVariable(hasRing, new Ink.Runtime.BoolValue(true));
}

    public override void UnequipItem()
    {
        Debug.Log("Ring unequipped");
        grandfatherStory.variablesState[hasRing] = false;
        DialogueManager.instance.dialogueVariables.ChangeVariable(hasRing, new Ink.Runtime.BoolValue(false));
    }

}
