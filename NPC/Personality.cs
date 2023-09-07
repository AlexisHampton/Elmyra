using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Personality
{

    [SerializeField] private List<TraitSO> personalityTraits = new List<TraitSO>();

    public List<TraitSO> PersonalityTraits { get { return personalityTraits; } }

    public Personality() { }

    public Personality(List<TraitSO> traits)
    {
        personalityTraits = traits;
    }

    public void AddTrait(TraitSO trait)
    {
        //if not full
        personalityTraits.Add(trait);
    }

        
}
