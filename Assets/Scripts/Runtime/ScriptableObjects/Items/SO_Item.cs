using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Food,
    Equipment,
    Default,
    Clothes,
    Weapons,
    Null
}

public enum Attributes
{
    stamina,
    health
}

public abstract class SO_Item : ScriptableObject
{
    public int id;
    public GameObject itemPrefabOnHands;
    public GameObject itemPrefabOnWorld;
    public Sprite uiDisplay;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    // public ItemType itemType;
    public SO_Item soItem;
    public GameObject itemPrefabOnHands;
    public GameObject itemPrefabOnWorld;
    public string name;
    public int id;
    public ItemBuff[] buffs;
    public Item(SO_Item soItem)
    {
        this.soItem = soItem;
        itemPrefabOnHands = soItem.itemPrefabOnHands;
        itemPrefabOnWorld = soItem.itemPrefabOnWorld;
        name = soItem.name;
        id = soItem.id;
        buffs = new ItemBuff[soItem.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(soItem.buffs[i].min, soItem.buffs[i].max);
            buffs[i].attribute = soItem.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}
