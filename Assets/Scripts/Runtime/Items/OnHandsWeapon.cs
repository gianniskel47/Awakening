using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHandsWeapon : MonoBehaviour
{
    public SO_Weapons soWeapon;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
