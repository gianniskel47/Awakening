using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static HandleAttack;

public class HandleItems : NetworkBehaviour
{
    [Header("SO")]
    [SerializeField] InputReader soInputReader;
    [SerializeField] GameEvent pickUpEvent;
    [SerializeField] GameEvent onLootCheck;
    [SerializeField] GameEvent updateInventorySlotsEvent;
    [SerializeField] GameEvent updateHandsSlotsEvent;
    [SerializeField] SO_Inventory soHandsInventory;
    [SerializeField] SO_Inventory soInventory;

    [SerializeField] SO_Vect2 soVect2;

    private Vector2 mousePos;
    [SerializeField] LayerMask pickableLayer;
    [SerializeField] LayerMask lootableLayer;

  //  public int playerIndex; //set when player spawns from GameManager

    [SerializeField] Transform itemHolderRightHand;
    [SerializeField] Transform itemHolderLeftHand;

    AnimationController animationController;
    HandleEquipedItems handleEquipedItems;

    [SerializeField] HandleAttack handleAttack; // SERVER ONLY VAR

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        soInputReader.InteractEvent += SoInputReader_InteractEvent;
        soInputReader.LookEvent += SoInputReader_LookEvent;

        animationController = GetComponentInChildren<AnimationController>();
        handleEquipedItems = GetComponent<HandleEquipedItems>();
        //  handleAttack = GetComponentInChildren<HandleAttack>();

        handleEquipedItems.equipItemFromEquipedItems += HandleEquipedItems_equipItemFromEquipedItems;
    }

    private void HandleEquipedItems_equipItemFromEquipedItems(object sender, HandleEquipedItems.EquipItemFromEquipedItemsEventArgs e)
    {
        EquipItemFromEquipedItems(e.soItem);
    }

    private void Update()
    {
        if (!IsOwner) return;

        soVect2.playerPos = transform.position;

       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            soInventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            soInventory.Load();
        }*/
    }

    private void SoInputReader_LookEvent(Vector2 arg0)
    {
        mousePos = arg0;
    }

    private void SoInputReader_InteractEvent()
    {
        if (!IsOwner) return;

        CheckForPickUp();
        CheckForLoot();
    }

    private void CheckForPickUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, pickableLayer))
        {
            ItemController itemControllerHit = hit.transform.GetComponent<ItemController>();

            if (!itemControllerHit.canBePickedUp) return;

            soInventory.AddItem(new Item(itemControllerHit.soItem), 1);
            pickUpEvent.Raise(null, null);

            DestroyWorldObjectServerRpc(hit.transform.GetComponent<NetworkObject>());
        }
    }

    private void CheckForLoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, lootableLayer))
        {
            LootHolder lootHolderHit = hit.transform.GetComponent<LootHolder>();

            if (lootHolderHit == null) return;

            if (!lootHolderHit.canSeeLoot || lootHolderHit.isSeeingLoot.Value) return;

            ChangeParamsServerRpc(lootHolderHit.GetComponent<NetworkObject>());

            onLootCheck.Raise(null, hit.transform.GetComponent<LootHolder>());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeParamsServerRpc(NetworkObjectReference networkObjRef, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId; //get client id who called the server rpc

        networkObjRef.TryGet(out NetworkObject itemNetworkObj);

        LootHolder lootHolder = itemNetworkObj.GetComponent<LootHolder>();

        lootHolder.isSeeingLoot.Value = true;
        lootHolder.playerIndex.Value = (int)clientId;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyWorldObjectServerRpc(NetworkObjectReference itemNetworkObjectReference)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);

      //  ItemController itemController = itemNetworkObject.GetComponent<ItemController>();

      //  itemController.DestroySelf();

        itemNetworkObject.Despawn(true);
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyHandsObjectServerRpc(NetworkObjectReference itemNetworkObjectReference)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObject);

        // OnHandsWeapon onHandsWeapon = itemNetworkObject.GetComponent<OnHandsWeapon>();

        // onHandsWeapon.DestroySelf();

        itemNetworkObject.Despawn(true);

    }

    //put the item from inventory to EquipedItems and Spawn it
    public void EquipItem(Component component, object data)
    {
        if (!IsOwner) return;

        if (handleEquipedItems.HasTwoWeapons())
        {
            Debug.Log("YOU HAVE 2 WEAPONS");
            return;
        }

        InventorySlot inventorySlot = (InventorySlot)data;

        EquipItemServerRpc(GetItemId(inventorySlot.item.soItem));

        handleEquipedItems.AddEquipedItem(inventorySlot);

        if (inventorySlot.item.soItem.type == ItemType.Weapons)
        {
            updateHandsSlotsEvent.Raise(null, null);
        }

        soInventory.RemoveItem(inventorySlot.item);

        updateInventorySlotsEvent.Raise(null, null);
    }

    //spawn the item from Equiped Items
    public void EquipItemFromEquipedItems(SO_Item soItem)
    {
        if (!IsOwner) return;

        EquipItemServerRpc(GetItemId(soItem));
    }

    [ServerRpc(RequireOwnership = false)]
    public void EquipItemServerRpc(int itemId, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId; //get client id who called the server rpc

        SO_Item soItem = GetSoItem(itemId);

        Transform itemTransform = Instantiate(soItem.itemPrefabOnHands).transform;

        NetworkObject itemTransformNetworkObj = itemTransform.GetComponent<NetworkObject>();

        itemTransformNetworkObj.Spawn(true);


        SetItemFollowClientRpc(itemTransformNetworkObj);
        UpdateClientClientRpc(itemTransformNetworkObj, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new ulong[] { clientId } } });

        for (int i = 0; i < handleAttack.currentlyEquipedWeaponsList.Count; i++)
        {
            if (handleAttack.currentlyEquipedWeaponsList[i].clientId == clientId)
            {
                handleAttack.currentlyEquipedWeaponsList[i].soWeapon = (SO_Weapons)soItem;
                handleAttack.currentlyEquipedWeaponsList[i].currentlyEquipedObject = itemTransform.gameObject;
                return;
            }
        }

        CurrentlyEquipedWeapons currentlyEquipedWeapons = new CurrentlyEquipedWeapons { soWeapon = (SO_Weapons)soItem, clientId = clientId, currentlyEquipedObject = itemTransform.gameObject };

        handleAttack.currentlyEquipedWeaponsList.Add(currentlyEquipedWeapons);
    }

    [ClientRpc]
    public void SetItemFollowClientRpc(NetworkObjectReference itemNetworkObjectReference)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObj);

        Transform itemTransform = itemNetworkObj.transform;

        SO_Weapons soWeapon = itemTransform.GetComponent<OnHandsWeapon>().soWeapon;

        if (soWeapon.isLeftHanded)
        {
            itemTransform.GetComponent<FollowTransform>().SetTargetTransform(itemHolderLeftHand);
        }
        else
        {
            itemTransform.GetComponent<FollowTransform>().SetTargetTransform(itemHolderRightHand);
        }
    }

    [ClientRpc]
    public void UpdateClientClientRpc(NetworkObjectReference itemNetworkObjectReference, ClientRpcParams clientRpcParams = default)
    {
        itemNetworkObjectReference.TryGet(out NetworkObject itemNetworkObj);

        Transform itemTransform = itemNetworkObj.transform;

        SO_Weapons soWeapon = itemTransform.GetComponent<OnHandsWeapon>().soWeapon;

        HandleEquipedItems handleEquipedItems = GetComponent<HandleEquipedItems>();

        if (handleEquipedItems.currentlyEquipedObject != null)
        {
            DestroyHandsObjectServerRpc(handleEquipedItems.currentlyEquipedObject.transform.GetComponent<NetworkObject>());
        }

        handleEquipedItems.currentlyEquipedObject = itemTransform.gameObject;
        handleEquipedItems.currentlyEquipedItem = soWeapon;

        animationController.SetWeaponAnimations(soWeapon.weaponType);
    }

    //na dropparei item apo inventory
    public void SpawnItem(Component component, object data)
    {
        if (!IsOwner) return;

        Item item = (Item)data;

        SpawnItemServerRpc(GetItemId(item.soItem));
    }

    // na dropparei equiped item    THELEI ALLAGH, AN DROPAREIS TO ALLO OPLO APO TO EQUIPED KSANA SPAWNAREI TO EQUIPED KAI EINAI SAN NA MHN FAINETAI
    public void SpawnItemFromEquiped(Component component, object data)
    {
        if (!IsOwner) return;

        Item item = (Item)data;

        SpawnItemServerRpc(GetItemId(item.soItem));

        DestroyHandsObjectServerRpc(handleEquipedItems.currentlyEquipedObject.transform.GetComponent<NetworkObject>());
        handleEquipedItems.currentlyEquipedObject = null;
        handleEquipedItems.currentlyEquipedItem = null;

        soHandsInventory.RemoveItem(item);

        if (handleEquipedItems.HasNoWeapons())
        {
            handleEquipedItems.currentlyEquipedItem = handleEquipedItems.soWeaponUnarmed;
            animationController.SetWeaponAnimations(WeaponType.Hands);
        }
        else
        {
            for (int i = 0; i < handleEquipedItems.soHandsInventory.container.items.Length; i++)
            {
                if (handleEquipedItems.soHandsInventory.container.items[i].item != null)
                {
                    EquipItemServerRpc(GetItemId(handleEquipedItems.soHandsInventory.container.items[i].item.soItem));
                }
            } 
        }
    }

    // na valei hands item pisw sto inventory     THELEI ALLAGH, AN DROPAREIS TO ALLO OPLO APO TO EQUIPED KSANA SPAWNAREI TO EQUIPED KAI EINAI SAN NA MHN FAINETAI
    public void DestroyItemFromStoring(Component component, object data)
    {
        if (!IsOwner) return;

        Item item = (Item)data;

        DestroyHandsObjectServerRpc(handleEquipedItems.currentlyEquipedObject.transform.GetComponent<NetworkObject>());
        handleEquipedItems.currentlyEquipedObject = null;
        handleEquipedItems.currentlyEquipedItem = null;

        soHandsInventory.RemoveItem(item);

        if (handleEquipedItems.HasNoWeapons())
        {
            handleEquipedItems.currentlyEquipedItem = handleEquipedItems.soWeaponUnarmed;
            animationController.SetWeaponAnimations(WeaponType.Hands);
        }
        else
        {
            for (int i = 0; i < handleEquipedItems.soHandsInventory.container.items.Length; i++)
            {
                if (handleEquipedItems.soHandsInventory.container.items[i].item != null)
                {
                    EquipItemServerRpc(GetItemId(handleEquipedItems.soHandsInventory.container.items[i].item.soItem));
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemServerRpc(int itemId)
    {
        SO_Item soItem = GetSoItem(itemId);

        Transform itemTransform = Instantiate(soItem.itemPrefabOnWorld, transform.position, Quaternion.identity).transform;

        NetworkObject itemTransformNetworkObj = itemTransform.GetComponent<NetworkObject>();

        itemTransformNetworkObj.Spawn(true);
    }

    public int GetItemId(SO_Item soItem)
    {
        for (int i = 0; i < soInventory.soItemDataBase.items.Length; i++)
        {
            if (soItem == soInventory.soItemDataBase.items[i])
            {
                return soInventory.soItemDataBase.items[i].id;
            }
        }
        return -1;
    }

    private SO_Item GetSoItem(int itemId)
    {
        for (int i = 0; i < soInventory.soItemDataBase.items.Length; i++)
        {
            if (itemId == soInventory.soItemDataBase.items[i].id)
            {
                return soInventory.soItemDataBase.items[i];
            }
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<ItemController>())
        {
            other.transform.GetComponent<ItemController>().canBePickedUp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<ItemController>())
        {
            other.transform.GetComponent<ItemController>().canBePickedUp = false;
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("QUITTTTTT");
        soInventory.container.items = new InventorySlot[12];
    }
}
