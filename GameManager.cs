using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the master script that manages the entire game
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //how close an Character can be before they collide into something
    [SerializeField] private float maxCollisionDistance = 0.2f;
    [SerializeField] private LayerMask charCollisionLayerMask; //what they can collide with
    [SerializeField] private List<SkillSO> skillSOs; //all of the skillSOs, since we don't yet have a skill manager class

    //getters
    public float MaxCollisionDistance { get { return maxCollisionDistance; } }
    public LayerMask CharCollisionLayerMask { get { return charCollisionLayerMask; } }


    private void Awake()
    {
        Instance = this;
    }

    //when the game starts, create jobs, add them to stores and initialize all npcs
    private void Start()
    {
        JobManager.Instance.SplitJobs();
        StoreManager.Instance.PopulateJobsForStores();
        NPCManager.Instance.InitNPCs();
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

    
}
