using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tasks that will be used when the npc is scheduled for job time
public class JobTask : Task
{
    [SerializeField] private JobSO jobSO;

    public JobSO GetJobSO { get { return jobSO; } }
}
