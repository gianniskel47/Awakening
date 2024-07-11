using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New IntChannelEvent",menuName = "EventsChannels/IntChannelEvent")]
public class IntChannelEvent : DisriptionSO{
    public event UnityAction<int> OnEventRaised;

    public void RaiseEvent(int value) {
        if (OnEventRaised != null) {
            OnEventRaised.Invoke(value);
        } else {
            Debug.LogWarning("No listeners have found. Possible listeners: " + Listener);
        }
    }
}

