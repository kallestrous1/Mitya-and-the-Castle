using Ink.Runtime;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.UnityIntegration;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Global Ink File")]
    [SerializeField] private InkFile globalsInkFile;
    public bool dialogueIsPlaying { get; private set; }

    private Story currentStory;
    private string currentStoryName;
    private string currentStorySavePath = null;

    public static DialogueManager instance;

    public DialogueVariables dialogueVariables;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("creating another dialoguemanager (not good)");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(globalsInkFile.filePath);
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            ContinueStory();
        }
//TODO
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            StartCoroutine(ExitDialogueMode());
        }

    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        currentStoryName = inkJSON.name;
        currentStory = DeserializeCurrentStory(ref currentStory);
        dialogueVariables.VariablesToStory(currentStory);
        currentStorySavePath = Application.persistentDataPath + "/" + currentStoryName + "currentStoryState.json";

        dialogueVariables.StartListening(currentStory);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            dialogueText.text = currentStory.currentText;
            Debug.Log("can continue is false");
            //StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("exiting dialoguemode, exiting "+ currentStoryName);

        SerializeCurrentStory(currentStory);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void DisplayChoices()
    {
        List<Choice> currentchoices = currentStory.currentChoices;

        if(currentchoices.Count > choices.Length)
        {
            Debug.LogError("More choices than ui can support. Number of choices given: " + currentchoices.Count);
        }

        int index = 0;
        foreach(Choice choice in currentchoices)
        {
            choices[index].gameObject.transform.parent.gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.transform.parent.gameObject.SetActive(false);
           // choices[i].gameObject.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log(choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    public static DialogueManager getInstance()
    {
        return instance;
    }

    private void SerializeCurrentStory(Story currentStory)
    {
        File.WriteAllText(currentStorySavePath, currentStory.state.ToJson());
    }

    private Story DeserializeCurrentStory(ref Story enteredStory)
    {
        // Create internal JSON string.
        string JSONContents;

        // Does the file exist?
        if (File.Exists(currentStorySavePath))
        {
            Debug.Log("story found at " + currentStorySavePath);
            // Read the entire file.
            JSONContents = File.ReadAllText(currentStorySavePath);
            // Overwrite current Story based on saved StoryState data.
            enteredStory.state.LoadJson(JSONContents);
        }
        else
        {
            Debug.Log("story save file not found");
        }

        return enteredStory;
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.inkVariables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null: " + variableName);
        }

        return variableValue;
    }


}
