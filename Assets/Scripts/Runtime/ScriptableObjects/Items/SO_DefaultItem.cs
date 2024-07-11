using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class SO_DefaultItem : SO_Item
{
    private void Reset()
    {
        type = ItemType.Default;
    }
}
