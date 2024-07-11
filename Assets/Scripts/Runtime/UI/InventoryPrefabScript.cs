using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPrefabScript : MonoBehaviour, IPointerClickHandler
{
    public ItemType itemType;
    public InventorySlot inventorySlot;

    [Header("Events")]
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    [Header("Prefabs")]
    [SerializeField] GameObject foodPanel;
    [SerializeField] GameObject clothesPanel;
    [SerializeField] GameObject equipmentPanel;
    [SerializeField] GameObject defaultPanel;

    [Header("SO")]
    [SerializeField] InputReader inputReader;
    [SerializeField] GameEvent equipItemEvent;

    // private GameObject currentPanel;
    private InventoryUI inventoryUI;
    private Vector2 mousePos;

    private Image image;

    // TEST
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;

    private void Start()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
        itemType = ItemType.Null;
        inputReader.LookEvent += InputReader_LookEvent;
        inputReader.InteractEvent += InputReader_InteractEvent;

        m_Raycaster = GetComponentInParent<GraphicRaycaster>();
        m_EventSystem = GetComponentInParent<EventSystem>();

        image = transform.GetChild(0).GetComponent<Image>();
    }

    private void InputReader_InteractEvent()
    {
         ClickToCloseActionPanel();
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
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

    public void ShowActionPanel()
    {
        if(inventoryUI.currentPanel != null)
        {
            Destroy(inventoryUI.currentPanel);
            inventoryUI.currentPanel = null;
        }

        switch (itemType) 
        {
            case ItemType.Null:
                return;
            case ItemType.Clothes:
                inventoryUI.currentPanel = Instantiate(clothesPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Equipment:
                inventoryUI.currentPanel = Instantiate(equipmentPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Default:
                inventoryUI.currentPanel = Instantiate(defaultPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Food:
                inventoryUI.currentPanel = Instantiate(foodPanel, mousePos, Quaternion.identity, transform);
                break;
            case ItemType.Weapons:
                inventoryUI.currentPanel = Instantiate(equipmentPanel, mousePos, Quaternion.identity, transform);    //NEEDS ITS OWN PANEL
                break;
        }
    }

    public void EquipItem()
    {
        if (inventoryUI.currentPanel != null && itemType != ItemType.Null && inventorySlot != null)
        {
            if(inventorySlot.item.soItem.type == ItemType.Food)
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);

                itemType = ItemType.Null;
            }
            
            equipItemEvent.Raise(null, inventorySlot);
            Destroy(inventoryUI.currentPanel);
            inventoryUI.currentPanel = null;
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
}
