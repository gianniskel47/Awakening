using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New FloatChannelEvent", menuName = "EventsChannels/FloatChannelEvent")]
public class FloatChannelEvent : DisriptionSO {
    public event UnityAction<float> OnEventRaised;

    public void RaiseEvent(float value) {
        if (OnEventRaised != null) {
            OnEventRaised.Invoke(value);
        } else {
            Debug.LogWarning("No listeners have found. Possible listeners: " + Listener);
        }
    }
}

