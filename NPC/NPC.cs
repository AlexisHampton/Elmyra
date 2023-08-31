using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//handles all the logic for a single npc 
public class NPC : Character
{
    //the ai that lets the npc walk
    [SerializeField] private NavMeshAgent agent;
    //the schdedule created by the goap that is not tweaked in the inspector, only looked at
    [SerializeField] private Schedule schedule;
    //the store the npc works at, I think. for testing, will go away when proper systems are in place
    [SerializeField] private Store store;

    //getters
    public Schedule GetSchedule { get { return schedule; } }


    private bool canMove;
    private Task currentTask;
    //how far a long an npc is in doing a task
    private int currentTaskDuration = 0;

    //creates the schedule for the npc
    public void SetSchedule()
    {
        schedule = GOAP.Instance.MakeSchedule(this);
    }

    //gets the npc a job
    public void ObtainJob()
    {
        //look for jobs in area, that match dream asp,
        //if none: do highest skill
        job = store.Jobs[2];
    }

    //finds and moves the npc to a task for a given schedule block index
    public void GoToTask(int index)
    {
        //if we are doing a task
        if (currentTask)
        {
            //if our task is not done, finish it and update the schedule
            if (!currentTask.IsDone(currentTaskDuration))
            {
                currentTaskDuration++;
                schedule.SetTask(currentTask, index);
            }
            else //if task is done
            {
                FinishTask(); //complete it
                currentTask = schedule.GetTask(index); //get a new task
                if (currentTask == null) //if none in the schedule
                {
                    //get task from goap and add it to the schedule
                    currentTask = GOAP.Instance.GetTask(this, index, schedule.GetScheduleType(index));
                    schedule.SetTask(currentTask, index);
                }
            }
        }
        else //if no task, find a task
        {
            currentTask = GOAP.Instance.GetTask(this, index, schedule.GetScheduleType(index));
            schedule.SetTask(currentTask, index);
        }
        
        currentTask.SetNPC(this);
        Debug.Log(name + " is doing: " + currentTask.name);
        Move(currentTask.Location.position);
    }


    private void DoTask()
    {
        //anim

    }

    //gives the npc the energy from the task, resets the CTD and lowers the need priority
    //since the need was satisfied
    private void FinishTask()
    {
        Debug.Log("Task Done" + currentTask.name);
        //energy up
        SetEnergy(currentTask.GetEnergy());
        currentTaskDuration = 0;

        //prioroty down
        if (currentTask.TaskNeed != Need.NULL)
            IncreasePriorityForNeed(currentTask.TaskNeed);
        currentTask = null;
    }

    public void AddToInventory()
    {

    }

    //moves the npc to a given position
    private void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    //can the npc move?
    public bool IsCanMove()
    {
        return canMove;
    }


}
