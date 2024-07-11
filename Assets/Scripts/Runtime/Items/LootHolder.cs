using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LootHolder : NetworkBehaviour
{

    public SO_Item[] holdingItems;
    public SO_Item[] holdingItemsClient;
    public int[] ids;

    [SerializeField] GameEvent leftFromLootCheck;

    [SerializeField] SO_ItemDataBase soItemDataBase;
    [SerializeField] ItemTypeToHold itemsToHold;

    private int itemCounter = 0;
    public int itemsToSpawn;

    public List<SO_Item> tempItems = new List<SO_Item>();

    public bool canSeeLoot = false;
    public NetworkVariable<bool> isSeeingLoot = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<int> playerIndex = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GetHowManyItemsToSpawn();
            SpawnItems(itemsToHold);

            SpawnItemsServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveItemServerRpc(int index)
    {
        RemoveItemClientRpc(index);
    }

    [ClientRpc]
    public void RemoveItemClientRpc(int index)
    {
        holdingItems[index] = null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemsServerRpc()
    {
        SpawnItemsClientRpc(ids);

    }

    [ClientRpc]
    public void SpawnItemsClientRpc(int[] ids)
    {
        if (IsServer) return;

        for (int i=0; i< ids.Length; i++)
        {
            if (ids[i] < 0)
            {
                continue;
            }

            holdingItems[i] = soItemDataBase.items[ids[i]];
        }
    }

    public void SpawnItems(ItemTypeToHold type)
    {
        if (itemsToSpawn == 0) return;

        if (type == ItemTypeToHold.all)
        {
            for(int i=0; i < holdingItems.Length; i++)
            {
                if (itemCounter >= itemsToSpawn) return;

                holdingItems[i] = soItemDataBase.items[Random.Range(0, soItemDataBase.items.Length - 1)];
                ids[i] = holdingItems[i].id;
                itemCounter++;
                Debug.Log(holdingItems[i].name);

            }

            return;
        }
        else if (type == ItemTypeToHold.food)
        {
            GetItems(ItemType.Food);

            PopulateHoldingItems();
        }
        else if (type == ItemTypeToHold.clothes)
        {
            GetItems(ItemType.Clothes);

            PopulateHoldingItems();
        }
        else if (type == ItemTypeToHold.equipments)
        {
            GetItems(ItemType.Equipment);

            PopulateHoldingItems();
        }
        else if (type == ItemTypeToHold.defaultItems)
        {
            GetItems(ItemType.Food);

            PopulateHoldingItems();
        } 
        else if (type == ItemTypeToHold.weapons)
        {
            GetItems(ItemType.Weapons);

            PopulateHoldingItems();
        }
    }

    public void PopulateHoldingItems()
    {
        for (int i = 0; i < holdingItems.Length; i++)
        {
            if (itemCounter >= itemsToSpawn) return;

            holdingItems[i] = tempItems[Random.Range(0, tempItems.Count)];
            itemCounter++;
            ids[i] = holdingItems[i].id;

            Debug.Log(holdingItems[i].name);
        }

        return;
    }

    public void GetHowManyItemsToSpawn()
    {
        itemsToSpawn = Random.Range(0, holdingItems.Length); 
    }

    public void GetItems(ItemType type)
    {
        for(int i=0; i < soItemDataBase.items.Length; i++)
        {
            if(type == soItemDataBase.items[i].type)
            {
                tempItems.Add(soItemDataBase.items[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSeeLoot = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerIndex.Value != (int)other.GetComponent<NetworkObject>().OwnerClientId) return;

            canSeeLoot = false;

            ChangeParamsServerRpc();
            leftFromLootCheck.Raise(null, null);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeParamsServerRpc()
    {
        isSeeingLoot.Value = false;
        playerIndex.Value = -1;
    }
}

public enum ItemTypeToHold
{
    all,
    food,
    clothes,
    equipments,
    defaultItems,
    weapons
}
