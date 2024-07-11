using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipedActionPanelScript : MonoBehaviour
{
    [SerializeField] Button actionButton;
    [SerializeField] Button dropButton;

    void Start()
    {
        actionButton.onClick.AddListener(() => GetComponentInParent<HandsPrefabScript>().StoreItem());
    }
}
