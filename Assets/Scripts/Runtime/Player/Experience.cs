using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints = 0;

    public event Action onExperienceGained;

    // on enemy death event response (enemy health.cs) and anywhere else to GetXP
    public void GainExperienceResponse(Component component, object sender)
    {
        float expReward = (float)sender;

        GainExperience(expReward);
    }

    private void GainExperience(float experience)
    {
        experiencePoints += experience;

        onExperienceGained();
    }

    public float GetPoints()
    {
        return experiencePoints;
    }
}
