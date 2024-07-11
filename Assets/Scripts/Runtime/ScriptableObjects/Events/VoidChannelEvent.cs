using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="EventsChannels/VoidChannelEvent")]
public class VoidChannelEvent : DisriptionSO
{
    public event UnityAction OnEventRaised;

    public void RaiseEvent() {
        if (OnEventRaised != null) {
            OnEventRaised.Invoke();
        } else {
            Debug.LogWarning("No listeners have found. Possible listeners: " + Listener);
        }
    }
}
