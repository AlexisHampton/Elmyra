using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//that job that a npc  or a store would have 
[System.Serializable]
public class Job
{
    [SerializeField] private string jobName;
    [SerializeField] private JobSO jobSO;
    [SerializeField] private Hours hours;
    [SerializeField] private Transform location;

    //getters
    public JobSO GetJobSO { get { return jobSO; } }
    public Hours GetHours { get { return hours; } set { hours = value; } }
    public Transform Location { get { return location; } }

    //creates a new job from a jobSO, but hours and location depend on the store it will go in
    public Job(JobSO jso)
    {
        jobSO = jso;
        hours = new Hours();
        jobName = jobSO.JobName;
    }

    //creates a job using a JobSO and hours 
    public Job(JobSO jso, Hours h)
    {
        jobSO = jso;
        hours = h;
        jobName = jso.name;
    }

    public void SetHours(Hours h)
    {
        hours = h;
    }

    public void SetLocation(Transform loc)
    {
        location = loc;
    }
}


