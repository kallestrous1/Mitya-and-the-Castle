using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "Golden Ring", menuName = "Inventory/Charms/Golden Ring")]

public class GrandfatherRing : ItemObject
{
    private string hasRing= "hasRing";
    Story grandfatherStory;

   
    public override void EquipItem()
    {
        grandfatherStory = new Story(itemStory.text);
        grandfatherStory.variablesState[hasRing] = true;
        DialogueManager.instance.dialogueVariables.ChangeVariable(hasRing, new Ink.Runtime.BoolValue(true));
}

    public override void UnequipItem()
    {
        grandfatherStory = new Story(itemStory.text);
        grandfatherStory.variablesState[hasRing] = false;
        DialogueManager.instance.dialogueVariables.ChangeVariable(hasRing, new Ink.Runtime.BoolValue(false));
    }

}
