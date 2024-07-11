using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootHolderPrefabUI : MonoBehaviour
{
    [SerializeField] SO_Inventory soInventory;

    [Header("Events")]
    [SerializeField] GameEvent pickUpEvent;

    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    public SO_Item soItem;

    [SerializeField] Image image;
    private int itemPosition;
    private LootHolder lootHolder;

    public void SetItem(SO_Item soItem)
    {
        this.soItem = soItem;
    }

    public void SetPosition(int index)
    {
        itemPosition = index;
    }

    public void SetLootHolder(LootHolder lootHolder)
    {
        this.lootHolder = lootHolder;
    }

    public void ClickItem()
    {
        if(soItem != null)
        {
            image.sprite = null;
            image.color = new Color(1, 1, 1, 0);

            soInventory.AddItem(new Item(soItem) ,1);
            pickUpEvent.Raise(null, null);
            lootHolder.RemoveItemServerRpc(itemPosition);

            soItem = null;
        }
    }
}
