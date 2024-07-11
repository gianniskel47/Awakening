using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] SO_Inventory soInventory;
    [SerializeField] InputReader inputReader;
    [SerializeField] SO_GameObj inventoryCanvas;

    public GameObject currentPanel;

    public MouseItem mouseItem = new MouseItem();
    private Vector2 mousePos;
    public GameObject inventoryPanel;
    public GameObject inventoryPrefab;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItem;
    public int numberOfColumn;
    public int ySpaceBetweenItem;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    public GameEvent dropItemEvent;

    void Start()
    {
        CreateSlots();
        inputReader.LookEvent += InputReader_LookEvent;
        inventoryCanvas.gameObject = this.gameObject;
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    public void UpdateSlots()
    {
        int i = 0;

        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = soInventory.soItemDataBase.GetItem[slot.Value.item.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1,1,1,1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");

                if (slot.Key.transform.GetComponent<InventoryPrefabScript>() != null)
                {
                    slot.Key.transform.GetComponent<InventoryPrefabScript>().itemType = soInventory.soItemDataBase.GetItem[slot.Value.item.id].type;

                    slot.Key.transform.GetComponent<InventoryPrefabScript>().inventorySlot = soInventory.container.items[i];
                }
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";

                if (slot.Key.transform.GetComponent<InventoryPrefabScript>() != null)
                {
                    slot.Key.transform.GetComponent<InventoryPrefabScript>().itemType = ItemType.Null;

                    slot.Key.transform.GetComponent<InventoryPrefabScript>().inventorySlot = null;
                }
            }

            i++;
        }
    }

    public void CreateSlots()
    {
       itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < soInventory.container.items.Length; i++) 
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, inventoryPanel.transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });


            itemsDisplayed.Add(obj, soInventory.container.items[i]);
        }
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform);

        if (itemsDisplayed[obj].id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = soInventory.soItemDataBase.GetItem[itemsDisplayed[obj].id].uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            soInventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
            dropItemEvent.Raise(null, itemsDisplayed[obj].item);

            soInventory.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(mouseItem.obj);
        mouseItem.item = null;
        UpdateSlots();
    }

    public void OnDrag(GameObject obj)
    {
        if(mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = mousePos;
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItem * (i % numberOfColumn)), yStart + (-ySpaceBetweenItem * (i/numberOfColumn)), 0f);
    }
}

public class MouseItem 
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}

