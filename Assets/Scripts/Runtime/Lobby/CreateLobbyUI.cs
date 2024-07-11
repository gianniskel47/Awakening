using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour
{
    public static CreateLobbyUI Instance { get; private set; }

    [SerializeField] GameObject changeLobbyNamePanel;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI publicPrivateText;
    [SerializeField] private TextMeshProUGUI maxPlayersText;

    private string lobbyName = "default lobby";
    private bool isPrivate = false;
    private int maxPlayers = 4;

    private void Awake()
    {
        Instance = this;

    }

    public void ShowChangeLobbyNamePanel()
    {
        changeLobbyNamePanel.SetActive(true);
    }

    public void Cancel()
    {
        changeLobbyNamePanel.SetActive(false);
    }

    public void ConfirmChangeName()
    {
        if (changeLobbyNamePanel.GetComponentInChildren<TMP_InputField>().text == string.Empty) return;

        lobbyNameText.text = changeLobbyNamePanel.GetComponentInChildren<TMP_InputField>().text;
        lobbyName = lobbyNameText.text;
        changeLobbyNamePanel.SetActive(false);
    }

    public void ChangeIsPrivate()
    {
        isPrivate = !isPrivate;

        if (isPrivate)
        {
           publicPrivateText.text = "PRIVATE";
        }
        else
        {
            publicPrivateText.text = "PUBLIC";
        }
    }

    public void IncreaseMaxPlayers()
    {
        if(maxPlayers >= 4)
        {
            maxPlayers = 1;
        }
        else
        {
            maxPlayers++;
        }

        maxPlayersText.text = maxPlayers.ToString();
    }

    public void DecreaseMaxPlayers()
    {
        if(maxPlayers <= 1)
        {
            maxPlayers = 4;
        }
        else
        {
            maxPlayers--;
        }

        maxPlayersText.text = maxPlayers.ToString();
    }

    public void CreateLobby()
    {
        TestLobby.Instance.CreateLobby(lobbyName, maxPlayers, isPrivate);
    }
}
