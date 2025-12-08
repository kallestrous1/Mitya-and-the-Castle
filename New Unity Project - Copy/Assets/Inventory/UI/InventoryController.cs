using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    #region Singleton
    public static InventoryController instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    bool inventoryActive = false;
    public GameObject inventory;
    public GameObject[] inventoryPanels;
    public GameObject shop;
    [SerializeField] private GameObject ItemDetailDisplay;
    CanvasGroup inventoryCanvas;

    public AudioClip openInventory;
    public AudioClip closeInventory;

    private void Start()
    {
        inventoryCanvas = inventory.GetComponent<CanvasGroup>();

    }

    protected void OnEnable()
    {
        InputManager.Instance.InventoryPressed += OnInventoryPressed;
    }

    protected void OnDisable()
    {
        InputManager.Instance.InventoryPressed -= OnInventoryPressed;
    }

    private void OnInventoryPressed()
    {
        if (inventoryActive)
        {
            ExitInventory();
        }
        else if(GameStateManager.instance.gameState == GameState.Play)
        {
           EnterInventory();
        }
    }

    public void ExitInventory()
    {
        if (closeInventory)
        {
            AudioManager.Instance.Play(closeInventory);
        }
        CloseItemDetailDisplay();
        inventoryCanvas.interactable = false;
        inventoryCanvas.alpha = 0;
        GameStateManager.instance.ChangeState(GameState.Play);
        inventoryActive = false;
        Time.timeScale = 1.0f;
        foreach(GameObject panel in inventoryPanels)
        {
            panel.SetActive(inventoryActive);
        }
        // inventory.SetActive(inventoryActive);
    }

    public void EnterInventory()
    {
        if (openInventory)
        {
            AudioManager.Instance.Play(openInventory);
        }
        OpenItemDetailDisplay();
        inventoryCanvas.interactable = true;
        inventoryCanvas.alpha = 1;
        GameStateManager.instance.ChangeState(GameState.inventory);
        Time.timeScale = 0.0f;
        inventoryActive = true;
        foreach (GameObject panel in inventoryPanels)
        {
            panel.SetActive(inventoryActive);
        }
        //inventory.SetActive(inventoryActive);
    }

    public void CloseItemDetailDisplay()
    {
        ItemDetailDisplay.SetActive(false);
    }
    public void OpenItemDetailDisplay()
    {
        ItemDetailDisplay.SetActive(true);
    }

}
