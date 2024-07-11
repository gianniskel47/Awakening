using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class SO_ItemDataBase : ScriptableObject, ISerializationCallbackReceiver
{
    public SO_Item[] items;

    public Dictionary<int, SO_Item> GetItem = new Dictionary<int, SO_Item>();

    public void OnAfterDeserialize()
    {
        for (int i=0; i<items.Length; i++)
        {
            items[i].id = i;
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, SO_Item>();
    }
}
