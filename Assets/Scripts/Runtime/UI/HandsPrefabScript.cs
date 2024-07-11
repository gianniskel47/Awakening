using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandsPrefabScript : MonoBehaviour, IPointerClickHandler
{
    public ItemType itemType;
    public InventorySlot inventorySlot;

    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    [SerializeField] GameObject defaultPanel;
    [SerializeField] InputReader inputReader;

    private InventoryUI inventoryUI;
    private Vector2 mousePos;

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    [SerializeField] SO_Inventory handsInventory;
    [SerializeField] SO_Inventory playerInventory;

    [SerializeField] GameEvent updateHandsSlots;
    [SerializeField] GameEvent pickUpEvent;  //update inventory slots
    [SerializeField] GameEvent storeWeaponEvent;

    private void Start()
    {
        inputReader.LookEvent += InputReader_LookEvent;
        inputReader.InteractEvent += InputReader_InteractEvent;

        inventoryUI = GetComponentInParent<InventoryUI>();
        itemType = ItemType.Null;

        m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        m_EventSystem = GetComponentInParent<EventSystem>();
    }

    private void InputReader_InteractEvent()
    {
        ClickToCloseActionPanel();
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    public void StoreItem()
    {
        Destroy(inventoryUI.currentPanel);
        inventoryUI.currentPanel = null;

        playerInventory.AddItem(inventorySlot.item, 1);

        storeWeaponEvent.Raise(null, inventorySlot.item);

       // handsInventory.RemoveItem(inventorySlot.item);  THIS IS CALLED ON HANDLE ITEMS FROM ABOVE EVENT DUE TO AN ERROR

        updateHandsSlots.Raise(null, null);
        pickUpEvent.Raise(null, null);
    }

    public void ShowActionPanel()
    {
        if (inventoryUI.currentPanel != null)
        {
            Destroy(inventoryUI.currentPanel);
            inventoryUI.currentPanel = null;
        }

        switch (itemType)
        {
            case ItemType.Null:
                return;
            case ItemType.Clothes:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Equipment:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Default:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Food:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Weapons:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);    //NEEDS ITS OWN PANEL
                break;
        }
    }

    public void ClickToCloseActionPanel()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = mousePos;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count <= 0)
        {
            Destroy(inventoryUI.currentPanel);
            inventoryUI.currentPanel = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            middleClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();
        }
    }
}
