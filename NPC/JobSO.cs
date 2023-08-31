using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Jung Hoseok", menuName ="Job")]
//creates a scriptable object for jobs that simply contains all the information
//this job would need
public class JobSO : ScriptableObject
{
    [SerializeField] private string jobName;
    [SerializeField] private Money minPay;
    [SerializeField] private Money maxPay;
    [SerializeField] private JobSO promotion; 
    [SerializeField] private List<ReqSkill> reqSkills;
    [SerializeField] private PlaceType placeType; //what place it will spawn at
    //lower priority = higher likliness to get filled
    //these are the jobs that would need to be filled first if a new business was made during runtime
    [SerializeField] private int priority;

    //getters
    public string JobName { get { return jobName; } }
    public Money MinPay { get { return minPay; } }
    public Money MaxPay { get { return maxPay; } }
    public JobSO Promotion { get { return promotion; } }
    public List<ReqSkill> ReqSkills { get { return reqSkills; } }
    public PlaceType GetPlaceType { get { return placeType; } }
    public int Priority { get { return priority; } }

}
