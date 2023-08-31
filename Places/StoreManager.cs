using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages all the stores 
public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }
    [SerializeField] List<Store> allStores;

    private void Awake()
    {
        Instance = this;
    }

    //puts jobs in stores
    public void PopulateJobsForStores()
    {
        //adds all the stores into the stores list (will refactor)
        foreach (Store store in FindObjectsOfType<Store>())
            allStores.Add(store);

        //assign jobs to Stores based on Place Type
        List<JobsForPlace> jobsForPlaces = JobManager.Instance.JobsForPlaces;

        foreach (Store store in allStores)
        {
            //foreach job, find the ones that match the placetype and add the jobs to the stores jobs
            for (int i = 0; i < jobsForPlaces.Count; i++)
                if (store.GetPlaceType == jobsForPlaces[i].placeType)
                    store.AddToJobs(JobManager.SortJobs(jobsForPlaces[i].jobs));
        }
    }
}
