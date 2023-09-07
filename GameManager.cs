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

    

    
}
