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
    List<Choice> currentchoices;

    public static DialogueManager instance;

    public DialogueVariables dialogueVariables;

    public GameObject shopPanel;
    public InventoryController inventoryController;
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
        if (GameStateManager.instance.gameState != GameState.dialogue)
        {
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            ContinueStory();
        }

        if (Input.GetKeyDown(KeyCode.S)){
            StartCoroutine(ExitDialogueMode());
        }
        //KIND OF BUGGY, ONLY USED FOR GAMETESTING PURPOSES
       /* if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentStory.ResetState();
        }*/

    }

    public void EnterDialogueMode(TextAsset inkJSON, GameObject dialogueCue = null)
    {
        if(GameStateManager.instance.gameState == GameState.dialogue)
        {
            Debug.LogWarning("Trying to enter dialogue mode while already in dialogue mode");
            return;
        }
        if(GameStateManager.instance.gameState != GameState.Play)
        {
            return;
        }

        if(dialogueCue != null)
        {
            dialogueCue.SetActive(false);
        }
        currentStory = new Story(inkJSON.text);
        currentStoryName = inkJSON.name;

        currentStorySavePath = Application.persistentDataPath + "/" + currentStoryName + "currentStoryState.json";

        currentStory = DeserializeCurrentStory(ref currentStory);

        dialogueVariables.VariablesToStory(currentStory);

        dialogueVariables.StartListening(currentStory);

        GameStateManager.instance.ChangeState(GameState.dialogue);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        TelemetryManager.instance.LogEvent("Dialogue", "entering story: " + currentStoryName);


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
                shopController.SetShopInactive();
                break;
            case "OpenTownShop":
                shopController.SetShop(townLadyShop);
                inventoryController.OpenItemDetailDisplay();
                shopController.SetShopActive();
                break;

            case "OpenLittleWizardShop":
                shopController.SetShop(littleWizardShop);
                inventoryController.OpenItemDetailDisplay();
                shopController.SetShopActive();
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

    public void ForceExitDialogueMode()
    {
        StartCoroutine(ExitDialogueMode());
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("exiting dialoguemode, exiting "+ currentStoryName);
        TelemetryManager.instance.LogEvent("Dialogue", "exiting story: "+ currentStoryName);
        SerializeCurrentStory(currentStory);

        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        GameStateManager.instance.ChangeState(GameState.Play);
        dialoguePanel.SetActive(false);
        AudioManager.Instance.Play(exitDialogueMode);
        dialogueText.text = "";
        if(shopController.currentShop != null)
            shopController.SetShopInactive();
        inventoryController.CloseItemDetailDisplay();
    }

    private void DisplayChoices()
    {
        currentchoices = currentStory.currentChoices;

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
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        Debug.Log(choiceIndex);
        TelemetryManager.instance.LogEvent("Dialogue", "making choice: " + currentchoices[choiceIndex].text + " in story: " + currentStoryName);
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
        if (!File.Exists(currentStorySavePath))
        {
            Debug.Log("story save file not found");
            return enteredStory;
        }

        try
        {
            Debug.Log("story found at " + currentStorySavePath);
            string json = File.ReadAllText(currentStorySavePath);
            enteredStory.state.LoadJson(json);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(
                $"Ink save incompatible for '{currentStoryName}', resetting story.\n{e.Message}"
            );

            // Delete the broken save so it doesn't keep crashing
            File.Delete(currentStorySavePath);

            // Fresh state
            enteredStory.ResetState();
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
        Debug.Log("Resetting all stories");
        for (int i =0; i<allStories.Length; i++)
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
