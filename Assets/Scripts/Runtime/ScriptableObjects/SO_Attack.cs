using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Attack SO", menuName = "Inventory System/Items/AttackSO")]
public class SO_Attack : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
