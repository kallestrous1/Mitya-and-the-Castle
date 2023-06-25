using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using System.IO;
using UnityEngine;

public class DialogueVariables 
{
    public Dictionary<string, Ink.Runtime.Object> inkVariables { get; set; }



    //this compiles my ink variables at runtime, this is mostly QOL so I don't have to recompile everytime I make a change, this should be 
    // adjusted in a final build as optimization

    public DialogueVariables(string globalsFilePath)
    {
        string inkFileContents = File.ReadAllText(globalsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        inkVariables = new Dictionary<string, Ink.Runtime.Object>();

        //initialize the inkVariables dictionary with variables from the compiled file
        foreach(string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            inkVariables.Add(name, value);
            Debug.Log("initialized ink variable " + name + " = " + value);
        }
    }


    public void StartListening(Story story)
    {
        story.variablesState.variableChangedEvent += ChangeVariable;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= ChangeVariable;
    }

    public void ChangeVariable(string name, Ink.Runtime.Object value)
    {
        if (inkVariables.ContainsKey(name))
        {
            Debug.Log(name + " ink value was changed");
            inkVariables.Remove(name);
            inkVariables.Add(name, value);
        }
    }

    public void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in inkVariables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
            Debug.Log(story.variablesState);
        }
    }


}
