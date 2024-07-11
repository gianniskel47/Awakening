using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class SO_FoodItem : SO_Item
{
    private void Reset()
    {
        type = ItemType.Food;
    }
}
