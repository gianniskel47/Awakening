using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private Transform playerPrefab;
    [SerializeField] FollowPlayerScript followPlayerScript;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            /* if (playerTransform.GetComponent<NetworkObject>().IsOwner)
             {
                 followPlayerScript.SetPlayerTransform(playerTransform);
                 Debug.Log("ALL SET");
             }*/
            UpdateCameraClientRpc(playerTransform.GetComponent<NetworkObject>());
        }
    }

    [ClientRpc]
    private void UpdateCameraClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject networkObject);

        Transform playerTransform = networkObject.GetComponent<Transform>();

        if (networkObject.IsOwner)
        {
            followPlayerScript.SetPlayerTransform(playerTransform);
        }
    }

}
