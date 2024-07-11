using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
public class Progression : ScriptableObject
{
    [SerializeField] CharacterClass characterClass;
    [SerializeField] ProgressionStat[] stats;

    Dictionary<Stat, float[]> statLookupTable = null;

    public float GetStat(Stat stat, int level)
    {
        BuildLookUp();

        float[] levels = statLookupTable[stat];

        if(levels.Length < level)
        {
            return 0;
        }

        return levels[level - 1];

        /*foreach (ProgressionStat progressionStat in stats)
        {
            if (progressionStat.stat != stat) continue;

            if (progressionStat.levels.Length < level) continue;   //safety reasons

            return progressionStat.levels[level - 1];
        }

        return 0;*/
    }

    private void BuildLookUp()
    {
        if (statLookupTable != null) return;

        statLookupTable = new Dictionary<Stat, float[]>();

        foreach (ProgressionStat progressionStat in stats)
        {
            statLookupTable[progressionStat.stat] = progressionStat.levels;
        }
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public float[] levels;
    }
}
