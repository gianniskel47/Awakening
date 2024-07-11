using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class SO_EquipmentItem : SO_Item
{
    private void Reset()
    {
        type = ItemType.Equipment;
    }
}
