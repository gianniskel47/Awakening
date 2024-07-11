using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandleEquipedItems : NetworkBehaviour
{
    public event EventHandler<EquipItemFromEquipedItemsEventArgs> equipItemFromEquipedItems;
    public class EquipItemFromEquipedItemsEventArgs:EventArgs
    {
        public SO_Item soItem;
    }

    public SO_Inventory soHandsInventory;
    public SO_Weapons currentlyEquipedItem;
    public GameObject currentlyEquipedObject;

    [SerializeField] InputReader inputReader;

    public SO_Weapons soWeaponUnarmed;

    private void Start()
    {
        if (!IsOwner) return;

        inputReader.Equip1stWeapon += InputReader_Equip1stWeapon;
        inputReader.Equip2ndWeapon += InputReader_Equip2ndWeapon;

        currentlyEquipedItem = soWeaponUnarmed;
    }

    private void InputReader_Equip2ndWeapon()
    {
        if (soHandsInventory.container.items[1].id == -1 || soHandsInventory.container.items[1].item.soItem == currentlyEquipedItem) return;

        equipItemFromEquipedItems.Invoke(this, new EquipItemFromEquipedItemsEventArgs { soItem = soHandsInventory.container.items[1].item.soItem });
    }

    private void InputReader_Equip1stWeapon()
    {
        if (soHandsInventory.container.items[0].id == -1 || soHandsInventory.container.items[0].item.soItem == currentlyEquipedItem) return;

        equipItemFromEquipedItems.Invoke(this, new EquipItemFromEquipedItemsEventArgs { soItem = soHandsInventory.container.items[0].item.soItem });
    }

    //game event response at player // on equip event
    public void AddEquipedItem(InventorySlot invSlot)
    {
        for (int i=0; i<soHandsInventory.container.items.Length; i++)
        {
            if (soHandsInventory.container.items[i].id == -1)
            {
                soHandsInventory.AddItem(invSlot.item, 1);
                return;
            }
        }
    }

    public bool HasTwoWeapons()
    {
        for (int i = 0; i < soHandsInventory.container.items.Length; i++)
        {
            if (soHandsInventory.container.items[i].id == -1)
            {
                return false;
            }
        }
        return true;
    }

    public bool HasNoWeapons()
    {
        for (int i = 0; i < soHandsInventory.container.items.Length; i++)
        {
            if (soHandsInventory.container.items[i].id >= 0)
            {
                return false;
            }
        }
        return true;
    }
}
