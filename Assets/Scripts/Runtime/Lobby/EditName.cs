using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditName : MonoBehaviour
{
    public static EditName Instance { get; private set; }

  //  public event EventHandler OnNameChanged;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private GameObject nameInputField;

    private string playerName = "Papito";

    private void Awake()
    {
        Instance = this;

        GetComponent<Button>().onClick.AddListener(() => {
            nameInputField.SetActive(true);
        });

        playerNameText.text = playerName;
    }

    private void Start()
    {
       // OnNameChanged += EditPlayerName_OnNameChanged;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
