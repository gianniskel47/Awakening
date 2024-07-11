using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandsUI : MonoBehaviour
{
    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    [SerializeField] SO_Inventory soHandsInventory;
    [SerializeField] InputReader inputReader;
    [SerializeField] GameEvent dropEquipedItem;

    [SerializeField] GameObject handsPrefab;
    [SerializeField] GameObject handsPanel;

    public int xStart;
    public int yStart;
    public int xSpaceBetweenItem;
    public int numberOfColumn;
    public int ySpaceBetweenItem;


    public HandMouseItem mouseItem = new HandMouseItem();
    private Vector2 mousePos;

    void Start()
    {
        inputReader.LookEvent += InputReader_LookEvent;
        CreateSlots();
    }

    private void InputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < 2; i++)
        {
            var obj = Instantiate(handsPrefab, Vector3.zero, Quaternion.identity, handsPanel.transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });


            itemsDisplayed.Add(obj, soHandsInventory.container.items[i]);
        }
    }

    public void UpdateSlots()
    {
        int i = 0;

        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = soHandsInventory.soItemDataBase.GetItem[slot.Value.item.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");

                if (slot.Key.transform.GetComponent<HandsPrefabScript>() != null)
                {
                    slot.Key.transform.GetComponent<HandsPrefabScript>().itemType = soHandsInventory.soItemDataBase.GetItem[slot.Value.item.id].type;

                    slot.Key.transform.GetComponent<HandsPrefabScript>().inventorySlot = soHandsInventory.container.items[i];
                }
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";

                if (slot.Key.transform.GetComponent<HandsPrefabScript>() != null)
                {
                    slot.Key.transform.GetComponent<HandsPrefabScript>().itemType = ItemType.Null;

                    slot.Key.transform.GetComponent<HandsPrefabScript>().inventorySlot = null;
                }
            }
            i++;
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItem * (i % numberOfColumn)), yStart + (-ySpaceBetweenItem * (i / numberOfColumn)), 0f);
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
            img.sprite = soHandsInventory.soItemDataBase.GetItem[itemsDisplayed[obj].id].uiDisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (mouseItem.hoverObj)
        {
            soHandsInventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else
        {
          //  soHandsInventory.RemoveItem(itemsDisplayed[obj].item);


            dropEquipedItem.Raise(null, itemsDisplayed[obj].item);
        }

        Destroy(mouseItem.obj);
        mouseItem.item = null;
        UpdateSlots();
    }

    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = mousePos;
        }
    }
}
public class HandMouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
