using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetworkUI : MonoBehaviour
{
    [SerializeField] Button startClientButton;
    [SerializeField] Button startServerButton;
    [SerializeField] Button startHostButton;

    private void Awake() {
        startClientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            print("Client Started");
        });

        startServerButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            print("Server Started");
        });

        startHostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            print("Host Started"); 
        });
    }
}
