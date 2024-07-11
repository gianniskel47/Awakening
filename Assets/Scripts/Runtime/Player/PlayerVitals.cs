using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerVitals : NetworkBehaviour
{
    public event EventHandler OnTired;

    [Header("Broadcast channels")]
    [SerializeField] private FloatChannelEvent healthChangeChannel;
    private NetworkVariable<float> net_currentHealth = new NetworkVariable<float>();
    private float currentStamina = 0;
    public float CurrentStamina {
        get => currentStamina;
        set { 
             currentStamina += value;

            if(currentStamina > 100) {
                currentStamina = 100;
            }
            if(currentStamina < 0) {
                currentStamina = 0;
                OnTired(this, EventArgs.Empty);
            }

            if (currentStamina == 0) {
                //do something
            }
        }
    }

    public override void OnNetworkSpawn() {
        int maxHealth = 100;
        net_currentHealth.Value = maxHealth;
        healthChangeChannel.RaiseEvent(net_currentHealth.Value);

        int maxStamina = 100;
        currentStamina = maxStamina;
    }

    private void Update()
    {
       /* if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            int layerToHit = 7;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerToHit))
            {

                TakeDamage(10, hitInfo.transform.GetComponent<NetworkObject>().OwnerClientId);
            }
        }*/
    }

    #region Health
    public void TakeDamage(float damage,ulong clientId) {
        TakeDamageServerRpc(damage, clientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRpc(float damage,ulong clientId) {
        var playerVitals = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponent<PlayerVitals>();

        if (playerVitals != null && playerVitals.net_currentHealth.Value > 0) {
            net_currentHealth.Value -= damage;

        }

        healthChangeChannel.RaiseEvent(net_currentHealth.Value);

        ClientRpcParams clientRpcParams = new ClientRpcParams {
            Send = new ClientRpcSendParams {
                TargetClientIds = new[] { clientId }
            }

        };
        TakeDamageClientRpc(clientRpcParams);
    }

    [ClientRpc]
    private void TakeDamageClientRpc(ClientRpcParams clientRpcParams = default) {
        if (IsOwner) return;
        //edo boroume na valoume autos pou xtipiete na kani kati p.x. ena animation
    }
    #endregion

}
