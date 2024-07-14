using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] int startingLevel = 1;
    [SerializeField] Progression progression = null;
    Experience experience;

    [SerializeField] GameEvent updateLevelUI;
    [SerializeField] GameEvent updateStatsOnLevelUp;


    private int currentLevel = 0;

    private void Start()
    {
        experience = GetComponent<Experience>();

        currentLevel = CalculateLevel();

        if (experience != null)
        {
            experience.onExperienceGained += UpdateLevel;
        }
    }

    private void UpdateLevel()
    {
        int newLevel = CalculateLevel();

        if (newLevel > currentLevel)
        {
            currentLevel = newLevel;
            updateLevelUI.Raise(null, currentLevel);
            updateStatsOnLevelUp.Raise(null, progression);
            print("LEVELED UPPP");
        }
    }

    public float GetStat(Stat stat)
    {
        return progression.GetStat(stat, CalculateLevel());
    }

    public int CalculateLevel()
    {
        if (experience == null) return startingLevel;

       float currentXP = experience.GetPoints();

        int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp);

        for (int level = 1; level <= penultimateLevel; level++)
        {
            float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, level);

            if(XPToLevelUp > currentXP)
            {
                return level;
            }
        }

        return penultimateLevel + 1;
    }
}
