using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Slider healthSlider;

    private void Start()
    {
        levelText.text = "level: 1";
    }

    // response on UpdateLevelEvent on EnemyHealth.cs
    public void UpdateLevelText(Component component, object sender)
    {
        int level = (int)sender;

        levelText.text = "level: " + level;
    }

    public void SetMaxHealth(Component component, object sender)
    {
        float maxHealth = (float)sender;

        healthSlider.maxValue = maxHealth;
    }

    public void SetHealth(Component component, object sender)
    {
        float health = (float)sender;

        healthSlider.value = health;
    }
}
