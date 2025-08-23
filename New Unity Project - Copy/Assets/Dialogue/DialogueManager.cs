using Ink.Runtime;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;
    public bool dialogueIsPlaying { get; private set; }

    private Story currentStory;
    private string currentStoryName;
    private string currentStorySavePath = null;

    public static DialogueManager instance;

    public DialogueVariables dialogueVariables;

    public GameObject shopPanel;
    public ShopController shopController;

    public InventoryObject townLadyShop;
    public InventoryObject littleWizardShop;

    public TextAsset[] allStories;

    public AudioClip enterDialogueMode;
    public AudioClip exitDialogueMode;
    public AudioClip nextDialogue;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("creating another dialoguemanager (not good)");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
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

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            StartCoroutine(ExitDialogueMode());
        }
        //KIND OF BUGGY, ONLY USED FOR GAMETESTING PURPOSES
       /* if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentStory.ResetState();
        }*/

    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        currentStoryName = inkJSON.name;

        currentStorySavePath = Application.persistentDataPath + "/" + currentStoryName + "currentStoryState.json";

        currentStory = DeserializeCurrentStory(ref currentStory);

        dialogueVariables.VariablesToStory(currentStory);

        dialogueVariables.StartListening(currentStory);

        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        AudioManager.Instance.Play(enterDialogueMode);

        currentStory.BindExternalFunction("triggerGameEvent", (string eventName) =>
        {
            Debug.Log($"Ink triggered event: {eventName}");
            TriggerUnityEvent(eventName);
        });

        ContinueStory();
    }

    void TriggerUnityEvent(string eventName)
    {
        switch (eventName)
        {
            case "CloseShop":
                shopController.setShopInactive();
                break;
            case "OpenTownShop":
                shopController.SetShop(townLadyShop);
                shopController.setShopActive();

                break;

            case "OpenLittleWizardShop":
                shopController.SetShop(littleWizardShop);
                shopController.setShopActive();
                break;
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            AudioManager.Instance.Play(nextDialogue);
        }
        else
        {
            dialogueText.text = currentStory.currentText;
            Debug.Log("can continue is false");
            //StartCoroutine(ExitDialogueMode());
        }
        DisplayChoices();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("exiting dialoguemode, exiting "+ currentStoryName);

        SerializeCurrentStory(currentStory);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        AudioManager.Instance.Play(exitDialogueMode);
        dialogueText.text = "";
        shopController.setShopInactive();
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


    public void ResetAllStories()
    {
        for(int i =0; i<allStories.Length; i++)
        {
            Story story = new Story(allStories[i].text);
            currentStoryName = allStories[i].name;
            currentStorySavePath = Application.persistentDataPath + "/" + currentStoryName + "currentStoryState.json";
            story = DeserializeCurrentStory(ref story);
            story.ResetState();
            SerializeCurrentStory(story);
        }
    }


}
