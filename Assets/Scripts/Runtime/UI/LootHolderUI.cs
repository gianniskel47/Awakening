using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LootHolderUI : MonoBehaviour
{
    [SerializeField] GameObject inventoryPrefab;
    [SerializeField] SO_Inventory soLootHolderInventory;
    [SerializeField] GameObject lootHolderPanel;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItem;
    public int numberOfColumn;
    public int ySpaceBetweenItem;

    private List<GameObject> itemsDisplayedV2 = new List<GameObject>();

    LootHolder lootHolder;

    void Start()
    {
        CreateSlots();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < soLootHolderInventory.container.items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, lootHolderPanel.transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

           // itemsDisplayed.Add(obj, soLootHolderInventory.container.items[i]);
            itemsDisplayedV2.Add(obj);
        }
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    public void UpdateSlotsV2(Component sender, object data)
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        lootHolder = (LootHolder)data;

        ClearSlots();

        for(int i = 0; i < lootHolder.holdingItems.Length; i++)
        {
            if (lootHolder.holdingItems[i] == null) continue;

            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = lootHolder.holdingItems[i].uiDisplay;
            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            LootHolderPrefabUI lootHolderPrefabUI = itemsDisplayedV2[i].GetComponent<LootHolderPrefabUI>();

            lootHolderPrefabUI.SetItem(lootHolder.holdingItems[i]);
            lootHolderPrefabUI.SetPosition(i);
            lootHolderPrefabUI.SetLootHolder(lootHolder);
        }
    }
    public void ClearSlots()
    {
        for (int i = 0; i < itemsDisplayedV2.Count; i++)
        {
            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);

        }
    }

    //editor
    public void HideSlots()
    {
        transform.GetChild(0).transform.gameObject.SetActive(false);

        for (int i = 0; i < itemsDisplayedV2.Count; i++)
        {
            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            itemsDisplayedV2[i].transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);

        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItem * (i % numberOfColumn)), yStart + (-ySpaceBetweenItem * (i / numberOfColumn)), 0f);
    }
}
