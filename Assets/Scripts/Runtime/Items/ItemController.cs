using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public SO_Item soItem;
    public bool canBePickedUp = false;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
