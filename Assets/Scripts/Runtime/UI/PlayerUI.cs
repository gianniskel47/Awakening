using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlide;
    [SerializeField] private Slider staminaSlider;
    [Header("Listenig Channels")]
    [SerializeField] private FloatChannelEvent healthChangeChannel;

    // Start is called before the first frame update
    void Start()
    {
        healthChangeChannel.OnEventRaised += HealthChangeChannel_OnEventRaised;
    }

    private void HealthChangeChannel_OnEventRaised(float arg0) {
        healthSlide.value = arg0;
    }
}
