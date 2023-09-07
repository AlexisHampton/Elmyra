using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kim Namjoon", menuName = "Trait")]

//Scriptable object that holds the information for a single trait
public class TraitSO : ScriptableObject
{
    [SerializeField] private string trait;
    [SerializeField] private List<TraitSO> comptatibleTraits;
    [SerializeField] private List<TraitSO> oppositeTraits;

    //getters
    public string GetTrait { get { return trait; } }
    public List<TraitSO> CompatibleTraits { get { return comptatibleTraits; } }
    public List<TraitSO> OppositeTraits { get { return oppositeTraits; } }

}
