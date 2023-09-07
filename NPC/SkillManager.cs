using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles all the skill logic: getting, initializing
public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [SerializeField] private List<SkillSO> skillSOs; //all of the skillSOs

    private void Awake()
    {
        Instance = this;
    }

    //returns a list of 2 skills, with randomized levels and exps
    public List<Skill> GetSkills(int numSkills)
    {
        List<SkillSO> allSkills = new List<SkillSO>(skillSOs);

        List<Skill> newSkills = new List<Skill>();
        for (int i = 0; i < numSkills; i++)
        {
            SkillSO randSkill = allSkills[GetRandNum(allSkills)];
            Skill newSkill = new Skill(randSkill);
            newSkills.Add(newSkill);
        }

        return newSkills;
    }


    //returns a skillSO for a specified skill Type
    //if not found, ret null
    public SkillSO GetSkillSO(SkillType st)
    {
        foreach (SkillSO skillSO in skillSOs)
            if (skillSO.skillType == st)
                return skillSO;
        return null;
    }

    //finds a skill in a list of skills and returns it
    public Skill FindSkill(List<Skill> skills, Skill skill)
    {
        if (skills.Contains(skill))
            return skills[skills.IndexOf(skill)];
        return null;
    }

    //finds a ReqSkill in a list of skills and returns it
    public Skill FindSkill(List<Skill> skills, ReqSkill reqSkill)
    {
        foreach (Skill skill in skills)
            if (skill.GetSkillType == reqSkill.GetSkillType)
                return skill;
        return null;
    }

    private Skill BuildSkill(SkillSO skillSO)
    {
        Skill skill = new Skill(skillSO);

        return skill;
    }

    //returns a random number in range
    private int GetRandNum(List<SkillSO> poss)
    {
        return Random.Range(0, poss.Count);
    }

}
