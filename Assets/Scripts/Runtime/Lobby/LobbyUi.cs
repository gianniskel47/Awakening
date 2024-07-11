using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUi : MonoBehaviour
{
    public static LobbyUi Instance { get; private set; }

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject kickedPanel;
    [SerializeField] private GameObject startGameButton;

    [SerializeField] private Transform playerSingleTemplate;
    [SerializeField] private Transform container;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playerCountText;

    private void Awake()
    {
        Instance = this;

        playerSingleTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        TestLobby.Instance.OnJoinedLobby += UpdateLobbyEvent;
        TestLobby.Instance.OnJoinedLobbyUpdate += UpdateLobbyEvent;
        TestLobby.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
        TestLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnKickedFromLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        mainPanel.SetActive(true);
        kickedPanel.SetActive(true);
        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e)
    {
        ClearLobby();
        mainPanel.SetActive(true);
        Hide();
    }

    private void UpdateLobbyEvent(object sender, TestLobby.LobbyEventArgs e)
    {
        Show();
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(TestLobby.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        ClearLobby();

        foreach (Player player in lobby.Players)
        {
            Transform playerSingleTransform = Instantiate(playerSingleTemplate, container);
            playerSingleTransform.gameObject.SetActive(true);
            LobbyPlayerUI lobbyPlayerUI = playerSingleTransform.GetComponent<LobbyPlayerUI>();

            lobbyPlayerUI.SetKickPlayerButtonVisible(
                TestLobby.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerUI.UpdatePlayer(player);
        }

       // changeGameModeButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());

        lobbyNameText.text = lobby.Name;
        playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;

        Show();
        ShowStartButton();
    }

    private void ClearLobby()
    {
        foreach (Transform child in container)
        {
            if (child == playerSingleTemplate) continue;
            Destroy(child.gameObject);
        }
    }

    public void ShowStartButton()
    {
        if (TestLobby.Instance.IsLobbyHost())
        {
            startGameButton.SetActive(true);
        }
    }

    public void StartGame()
    {
        TestLobby.Instance.StartGame();
    }

    public void ReadyButton()
    {
        LobbyReady.Instance.SetPlayerReady();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
