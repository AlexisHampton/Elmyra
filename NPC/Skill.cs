using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { SINGING, DANCING, WRITING, PAINTING, CHARISMA, MUSIC, COMBAT }
//handles most skill logic
[System.Serializable]
public class Skill
{
    [SerializeField] private SkillType skillType;
    [SerializeField] private int currlevel;
    [SerializeField] private int levelEXP;
    [SerializeField] private int maxEXP;
    [SerializeField] private SkillSO skillSO;

    //getters
    public SkillType GetSkillType { get { return skillType; } }
    public int Level { get { return currlevel; } }
    public int LevelEXP { get { return levelEXP; } }
    public int MaxEXP { get { return maxEXP; } }


    //constuctor that builds a skill based on skill type, all other defaults are 0
    public Skill(SkillType st)
    {
        skillType = st;
        skillSO = GameManager.Instance.GetSkillSO(skillType);
        currlevel = 0;
        levelEXP = 0;
        maxEXP = skillSO.skillDefs[0].maxEXP;
    }

    //builds a skill with a skill tyype and current level
    public Skill(SkillType st, int lvl)
    {
        skillType = st;
        currlevel = lvl;
    }

    //increases skill experience 
    public void IncreaseSkillEXP(int exp)
    {
        levelEXP += exp;
        if (levelEXP >= maxEXP)
            IncreaseSkill();
    }

    //increases the skill level 
    public void IncreaseSkill()
    {
        if (skillSO.skillDefs[currlevel + 1] != null)
        {
            levelEXP -= maxEXP;
            currlevel++;
            maxEXP = skillSO.skillDefs[currlevel].maxEXP;
        }

    }

    //the next two functions will likely be in a skillmanager class when I get around to
    //implementing the skills again
    
    //finds a skill in a list of skills and returns it
    public static Skill FindSkill(List<Skill> skills, Skill skill)
    {
        if (skills.Contains(skill))
            return skills[skills.IndexOf(skill)];
        return null;
    }
     
    //finds a ReqSkill in a list of skills and returns it
    public static Skill FindSkill(List<Skill> skills, ReqSkill reqSkill)
    {
        foreach (Skill skill in skills)
            if (skill.skillType == reqSkill.GetSkillType)
                return skill;
        return null;
    }

    public bool Equals(Skill other)
    {
        if (skillType != other.skillType) return false;
        if (currlevel != other.currlevel) return false;
        if (levelEXP != other.levelEXP) return false;
        if (maxEXP != other.maxEXP) return false;
        if (skillSO != other.skillSO) return false;
        return true;
    }

    public bool Equals(ReqSkill other)
    {
        if (skillType != other.GetSkillType) return false;
        if (currlevel != other.ReqLevel) return false;
        return true;
    }

}

//used for jobs and items where only the skillType and required skill level matter
[System.Serializable]
public class ReqSkill
{
    [SerializeField] private SkillType skillType;
    [SerializeField] private int reqlevel;

    //getters
    public SkillType GetSkillType { get { return skillType; } }
    public int ReqLevel { get { return reqlevel; } }
}
