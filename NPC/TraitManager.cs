using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages traits and grants them to npcs 
public class TraitManager : MonoBehaviour
{
    public static TraitManager Instance { get; private set; }

    [SerializeField] private List<TraitSO> allTraits;
    [SerializeField] private int maxPersonalityTraits = 5;

    public int MaxPersonalityTraits { get { return maxPersonalityTraits; } }

    private void Awake()
    {
        Instance = this;
    }

    //returns true if the two given lists of Traits have at least one trait in common
    public bool IsCompatible(List<TraitSO> firstTraits, List<TraitSO> secondTraits)
    {
        //have at least one trait in common
        if (GetCompatability(firstTraits, secondTraits) >= (1/maxPersonalityTraits))
            return true;
        return false;
    }

    //returns a compatability between 0 and 1, which is determined by how many triats they share
    public float GetCompatability(List<TraitSO> firstTraits, List<TraitSO> secondTraits)
    {
        float compatability = 0;
        foreach (TraitSO traitSO in firstTraits)
            if (secondTraits.Contains(traitSO))
                compatability++;
        //normalize the compatability into a float
        return compatability / (float) maxPersonalityTraits;
    }

    //returns a list of traits 
    public List<TraitSO> GetTraits()
    {
        return GetTraits(new List<TraitSO>(allTraits), new List<TraitSO>());
    }

    //give traits, opposite ones not considered
    private List<TraitSO> GetTraits(List<TraitSO> possTraits, List<TraitSO> newTraits)
    {
        TraitSO consideringTrait = possTraits[GetRandNum(possTraits)];
        newTraits.Add(consideringTrait);
        possTraits.Remove(consideringTrait);
        //get rid of all the opposite traits
        foreach(TraitSO opp in consideringTrait.OppositeTraits)
            possTraits.Remove(opp);
        //if we have less then the max, get more traits 
        if (newTraits.Count < maxPersonalityTraits)
            return GetTraits(possTraits, newTraits);
        return newTraits;
    }

    //returns a random number in range
    private int GetRandNum(List<TraitSO> poss)
    {
        return Random.Range(0, poss.Count);
    }

    

    //add a new trait based on what npc likes??

}
