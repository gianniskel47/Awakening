using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListUi : MonoBehaviour
{
    public static LobbyListUi Instance { get; private set; }

    [SerializeField] private GameObject createLobbyUIPanel;
    [SerializeField] private Transform lobbySingleTemplate;
    [SerializeField] private Transform container;

    private float refreshCounter = 0;
    private float timeToRefresh = 2f;

    private void Awake()
    {
        Instance = this;

        lobbySingleTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        TestLobby.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        TestLobby.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        TestLobby.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        TestLobby.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }

    private void Update()
    {
        refreshCounter += Time.deltaTime;
        if(refreshCounter > 1000)
        {
            refreshCounter = 1000;
        }
    }

    private void LobbyManager_OnKickedFromLobby(object sender, TestLobby.LobbyEventArgs e)
    {
       // Show();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
       // Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, TestLobby.LobbyEventArgs e)
    {
        Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, TestLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in container)
        {
            if (child == lobbySingleTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, container);
            lobbySingleTransform.gameObject.SetActive(true);
            LobbyListSingleUi lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUi>();
            lobbyListSingleUI.UpdateLobby(lobby);
        }
    }

    public void RefreshButtonClick()
    {
        if(!(refreshCounter > timeToRefresh)) return;

        refreshCounter = 0;
        TestLobby.Instance.RefreshLobbyList();
    }

    public void CreateLobbyButtonClick()
    {
        createLobbyUIPanel.SetActive(true);
        gameObject.SetActive(false);
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
