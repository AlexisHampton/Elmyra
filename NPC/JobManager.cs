using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jimin's new pants 
public class JobManager : MonoBehaviour
{
    public static JobManager Instance { get; private set; }
    public static List<Job> allJobs { get; private set; }

    [SerializeField] private List<JobSO> allJobSOs;

    private List<JobsForPlace> jobsForPlaces = new List<JobsForPlace>();


    //getters
    public List<JobsForPlace> JobsForPlaces { get { return jobsForPlaces; } }

    private void Awake()
    {
        Instance = this;
    }

    //splits the jobs into each place type
    //so watchtower pt gets the patrol, captain, and officer jobs, but not the manager ones
    public void SplitJobs()
    {
        //split based on place type
        foreach(PlaceType placeType in Enum.GetValues(typeof(PlaceType)))
        {
            JobsForPlace jobsForPlace = new JobsForPlace(placeType);
            for(int i = 0; i < allJobSOs.Count; i++)
            {
                if (allJobSOs[i].GetPlaceType == placeType)
                {
                    //if pt match, add the job to the job list in jobsForPlace
                    jobsForPlace.jobs.Add(new Job(allJobSOs[i]));
                }
            }
            //add the completed jobsForPlaces into the global list
            jobsForPlaces.Add(jobsForPlace);
        }
    }

    //returns a sorted list of jobs given an unsorted list of jobs
    //i might delete this later, i've never seen a more useless function
    public static List<Job> SortJobs(List<Job> j)
    {
        List<Job> sortedJobs = new List<Job>();

        for(int i = 0; i < j.Count; i++)
            sortedJobs.Insert(i, j[i]);
        return sortedJobs; 
    }

}

//a class that contains all the jobs for a given place type

public class JobsForPlace
{
    public List<Job> jobs;
    public PlaceType placeType;

    public JobsForPlace(PlaceType pt)
    {
        jobs = new List<Job>();
        placeType = pt;
    }
}