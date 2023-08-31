using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a scriptable object that holds the skill type and all the skill levels and their maxEXPs
[CreateAssetMenu(fileName = "Min Yoongi", menuName = "Skill")]
public class SkillSO : ScriptableObject
{
    public SkillType skillType;
    public List<SkillDef> skillDefs;

    //returns the maxEXP for the current skill level 
    //returns -1 if not found
    public int GetMaxEXP(int level)
    {
        foreach (SkillDef skillDef in skillDefs)
            if (skillDef.level == level)
                return skillDef.maxEXP;
        return -1;
    }

}

//holds the maxEXP and the level
//ex: level 1 = 10 xp
//level 2 = 20xp (cumulative)
[System.Serializable]
public class SkillDef
{
    public int maxEXP;
    public int level;
}
