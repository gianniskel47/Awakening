using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clothes Object", menuName = "Inventory System/Items/Clothes")]
public class SO_Clothes : SO_Item
{
    private void Reset()
    {
        type = ItemType.Clothes;
    }
}
