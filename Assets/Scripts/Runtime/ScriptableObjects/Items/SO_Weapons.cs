using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clothes Object", menuName = "Inventory System/Items/Weapons")]
public class SO_Weapons : SO_Item
{
    public List<SO_Attack> soAttack;
    public GameObject projectilePrefab;
    public GameObject enemyProjectilePrefab;

    public WeaponType weaponType;
    public float damage = 5f;
    public float attackCooldown = 3f;
    public float weaponRange = 2f;
    public bool isLeftHanded = false;

    private void Reset()
    {
        type = ItemType.Weapons;
    }
}

public enum WeaponType
{
    Hands,
    MiniSword,
    Sword,
    BigSword,
    Bow,
    Wand
}

