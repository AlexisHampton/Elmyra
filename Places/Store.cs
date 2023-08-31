using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a single business, managed by storeManager
public class Store : Place
{
    [SerializeField] private List<Job> jobs;
    [SerializeField] private Hours hours;
    [SerializeField] private List<Employee> employees;
    [SerializeField] private int maxCapacity; //max employees

    //getters 
    public List<Job> Jobs { get { return jobs; } }
    public Hours GetHours { get { return hours; } }
    public List<Employee> Employees { get { return employees; } }
    public int MaxCapacity { get { return maxCapacity; } }

    private int jobIndex;//job that needs to be filled next

    //gives a job to a given character
    public void FillJob(Character chara)
    {
        if (jobIndex >= jobs.Count) return; //no more jobs
        HireCharacter(chara, jobs[jobIndex]);
        jobIndex++;
    }

    //will set the character to the job and makes them an employee
    void HireCharacter(Character character, Job job)
    {
        Money pay = CalculatePay(job.GetJobSO.MinPay, job.GetJobSO.MaxPay);
        employees.Add(new Employee(character, pay, job));
    }

    //calulates the pay depending on the min and max pay
    Money CalculatePay(Money min, Money max)
    {
        int randAmt = Random.Range(min.Amount, max.Amount);
        return new Money(randAmt, min.GetMoneyType);
    }

    //adds a list of jobs to the store's jobs and sets their hours and location
    public void AddToJobs(List<Job> j)
    {
        if (j == null) return;
        foreach (Job job in j) {
            job.SetHours(hours);
            job.SetLocation(transform);
            jobs.Add(job);
        }
    }
}

//creates an employee for a store
[System.Serializable]
public class Employee
{
    [SerializeField] private Character employee;
    [SerializeField] private Money pay;
    [SerializeField] private Job job;

    //getters
    public Character GetEmployee { get { return employee; } }
    public Money Pay {  get { return pay; } }
    public Job GetJob { get { return job; } }

    //constructor
    public Employee(Character e, Money p, Job j)
    {
        employee = e;
        pay = p;
        job = j;
    }

}
