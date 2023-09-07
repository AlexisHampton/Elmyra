using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//what the npc could be doing while completing this schedule
public enum ScheduleType { NEED, JOB, HOBBY, GOAL, SLEEP, EAT, HYGIENE, FUN, SOCIAL, NULL }

//the schedules the npcs use to manage their time
[Serializable]
public class Schedule
{
    //a chunk of schedule 
    [Serializable]
    public class ScheduleBlock
    {
        public string scheduleName; //just makes it easier to read in the inspector
        public Hours hours = new Hours(); // has start and end times
        public ScheduleType scheduleType;
        public Task task; //task the npc is completing
    }


    public List<ScheduleBlock> schedule;
    public int Count { get { return schedule.Count; } }

    private int index = 0;

    //initializes shedule using how long it takes for gameTime to increase, and how many schedule blocks we have
    public Schedule(int timeToNext, int scheduleBlocks)
    {
        schedule = new List<ScheduleBlock>();
        Clock clock = new Clock();
        //fills each scheduleBlock with a start and end time, a null schedule type and a null task
        for (int i = 0; i < scheduleBlocks; i++)
        {
            ScheduleBlock sb = new ScheduleBlock();
            sb.hours.startTime = new Clock(clock);
            sb.scheduleName = sb.hours.startTime.ToString();
            sb.hours.endTime = new Clock(clock.IncreaseTimeMin(timeToNext));
            sb.scheduleType = ScheduleType.NULL;
            sb.task = null;
            schedule.Add(sb);
        }
    }

    //sets the schedule block at specified index to a specific schedule type
    public void SetSchedule(int i, ScheduleType st)
    {
        schedule[i].scheduleType = st;
        schedule[i].scheduleName += " " + st;
    }

    //sets the schedule block to a given schedule type given a Clock
    //-1 if not exist
    public int SetSchedule(Clock time, ScheduleType st)
    {
        int i = GetIndex(time);
        if (i == -1) return -1;

        SetSchedule(i, st);
        return i;
    }

    //retrieves the index of the schedule block given a Clock 
    //-1 if not exist
    public int GetIndex(Clock i)
    {
        foreach (ScheduleBlock s in schedule)
        {
            if (s.hours.startTime == i)
                return schedule.IndexOf(s) % schedule.Count;
        }
        return -1;
    }

    //returns the task of the next index
    //(assumes that all days run from dusk to dawn so the index always increases linearly)
    public Task NextTask()
    {
        return schedule[index++].task;
    }

    //returns the task at specified index
    public Task GetTask(int i)
    {
        return schedule[i].task;
    }

    //returns the schedule type at a specified index
    public ScheduleType GetScheduleType(int i)
    {
        return schedule[i].scheduleType;
    }

    //sets the given task to a schedule block at a certain Clock time
    public void SetTask(Clock time, Task t)
    {
        int i = GetIndex(time);
        SetTask(t, i);
    }

    //sets the given task to a schedule block but at a certain index
    public void SetTask(Task task, int i)
    {
        if (schedule[i].task == null)
            schedule[i].task = task;
    }

}

//not a real GOAP, runs almost like one though
//creates the schedules for all the npcs and finds them tasks upon request
public class GOAP : MonoBehaviour
{
    public static GOAP Instance { get; private set; }

    //max distance from the npc that tasks are gettable
    [SerializeField] private float taskRadius = 15;
    [SerializeField] private LayerMask taskMask; //only "collide" with tasks

    //time already allocated in the schedule
    private List<int> timeTaken = new List<int>();
    private int scheduleMax = 0;
    //the max amount of tasks we can hit is 10
    private RaycastHit[] Hits = new RaycastHit[10];
    //scheduletype to Task type
    private Dictionary<ScheduleType, Type> scheduleToType = new Dictionary<ScheduleType, Type>();


    //inits the dictionary since we can't do that from the inspector
    private void Awake()
    {
        Instance = this;

        scheduleToType.Add(ScheduleType.NEED, typeof(NeedTask));
        scheduleToType.Add(ScheduleType.SLEEP, typeof(NeedTask));
        scheduleToType.Add(ScheduleType.HOBBY, typeof(HobbyTask));
        scheduleToType.Add(ScheduleType.JOB, typeof(JobTask));
        scheduleToType.Add(ScheduleType.GOAL, typeof(GoalTask));
        scheduleToType.Add(ScheduleType.NULL, typeof(JobTask));
    }

    //creates and returns a schedule for a given npc
    public Schedule MakeSchedule(NPC npc)
    {
        Schedule schedule = new Schedule(GameTime.Instance.MinutesToNextTime, GameTime.Instance.ScheduleBlocks);
        scheduleMax = schedule.Count;

        //we have to block in the job first
        Clock currTime = npc.GetJob.GetHours.startTime;
        //set the schedule type and keep the index for iterating
        int index = schedule.SetSchedule(currTime, ScheduleType.JOB);
        timeTaken.Add(index);

        //until we hit the end time, set the scheduleType to job
        while (currTime != npc.GetJob.GetHours.endTime)
        {
            index++;
            schedule.SetSchedule(index, ScheduleType.JOB);
            currTime = currTime.IncreaseTimeMin(GameTime.Instance.MinutesToNextTime);
            timeTaken.Add(index);
        }

        try
        {
            //sleep from 11-7 for now (in military time)
            PopulateSleep(16, 11 + 12, schedule);
            //fill in needs 
            PopulateSchedule(15, ScheduleType.NEED, schedule);
            //hobbies if time allows
            PopulateSchedule(4, ScheduleType.HOBBY, schedule);
            //the rest of time is dedicated to goals when we implement that

            //clear the list before the next npc gets here!!!
            timeTaken.Clear();
        }
        catch (IndexOutOfRangeException e)
        {
            //no more space in the array
            return schedule;
        }

        return schedule;
    }
    // ---------------Task stuff------------------//
    //returns a task for a given npc, schedule block and schedule type
    public Task GetTask(NPC npc, int index, ScheduleType st)
    {
        List<Task> possibleTasks = new List<Task>();

        //get all tasks within radius
        int hits = Physics.SphereCastNonAlloc(npc.transform.position, taskRadius, Vector3.forward, Hits, 0, taskMask);

        //get the tasks that match schedule type
        for (int i = 0; i < hits; i++)
        {
            //Debug.Log(npc.name + " hit:" + Hits[i].collider.name);
            Task poss = Hits[i].collider.GetComponent<Task>();
            //add possible task if there is a task, our shcedule type matches and no one is there
            if (poss != null && scheduleToType[st] == poss.GetType() && !poss.HasNPC())
            {
                Debug.Log(npc.name + " Schedule: " + st + " possible task: " + poss);
                possibleTasks.Add(poss);
            }
        }
        //clear Hits
        if (hits > 0)
            System.Array.Clear(Hits, 0, Hits.Length);
        //check if valid
        //look through valid ones based on score and
        //pick the best
        return GetBestTask(npc, possibleTasks, st);
        //return task;

    }

    //returns the best task for a given npc, list of possible tasks and the schedule type
    private Task GetBestTask(NPC npc, List<Task> tasks, ScheduleType st)
    {
        Dictionary<Task, float> taskScores = new Dictionary<Task, float>();

        //Get all the scores
        foreach (Task task in tasks)
            taskScores.Add(task, GetTaskScore(npc, st, task));

        Task best = tasks[0];
        float bestScore = taskScores[best];


        //the best is the one with the smallest value
        foreach (KeyValuePair<Task, float> taskScore in taskScores)
        {
            if (taskScore.Value < bestScore)
            {
                bestScore = taskScore.Value;
                best = taskScore.Key;
                Debug.Log("best: " + best.name + " " + bestScore);
            }
        }
        return best;
    }

    //returns a task score for a given task, npc and schedule type
    private float GetTaskScore(NPC npc, ScheduleType st, Task task)
    {
        
        //the ones that don't match the schedule types get the highest values
        switch (st)
        {
            //if the need does not match the lowest need
            case ScheduleType.NEED:
                if (npc.GetLowestNeed() != task.TaskNeed)
                    return float.MaxValue;
                break;
            //if the need is not sleep
            case ScheduleType.SLEEP:
                if (task.TaskNeed != Need.REST)
                    return float.MaxValue;
                break;
            //if the job does not match the npc's job
            case ScheduleType.JOB:
                //check job
                break;
            //if the npc does not do this hobby
            case ScheduleType.HOBBY:
                //check hobby
                break;
            //if this does not further the npc's goals
            case ScheduleType.GOAL:
                //check goal
                break;
            //if by God's wrath, schedule type is still null
            case ScheduleType.NULL:
                Debug.LogError("Task should not be null");
                break;
        }

        //likewise, those with 0 compatability get the highest value
        Debug.Log(npc.name + " compat: " + TraitManager.Instance.GetCompatability
            (npc.GetPersonality.PersonalityTraits, task.Traits) + " " + task.name);
        if (!TraitManager.Instance.IsCompatible
            (npc.GetPersonality.PersonalityTraits, task.Traits))
            return float.MaxValue;

        //check the skill level of the npc and of the task
        Debug.Log(npc.name + " skill " + IsValid(npc, task) + " taks: " + task.name);
        if (!IsValid(npc, task))
            return float.MaxValue;

        //otherwise give the tasks actual score
        return task.GetTaskScore(npc.transform.position);
    }

    //returns if a given task is valid for a given npc based on their skills and personality
    public bool IsValid(NPC npc, Task task)
    {
        //meet *all* skillreq
        foreach (ReqSkill reqSkill in task.ReqSkills)
        {
            Skill skill = SkillManager.Instance.FindSkill(npc.Skills, reqSkill);
            if (skill == null || !skill.Equals(reqSkill))
                return false;
        }
        return true;
    }



    // ---------------schdule stuff------------------//
    //returns a random index for the schedule given an upper limit
    //returns -1 if the schedule is full
    public int GetRandNum(int max)
    {
        //if the schedule is full
        if (scheduleMax == timeTaken.Count) return -1;

        //get random num and hope it isn't already taken
        int randNum = UnityEngine.Random.Range(0, max);
        if (timeTaken.Contains(randNum))
            return GetRandNum(max);
        return randNum;
    }

    //populates the sleep schedule for a given sleep start hour, schedule and the max amt of blocks to be alloted
    public void PopulateSleep(int max, int startSleepHour, Schedule schedule)
    {
        //gets the index for the first hour set to sleep
        int index = schedule.SetSchedule(new Clock(startSleepHour, 0), ScheduleType.SLEEP);
        //continue to fill in subsequent schedule blocks until we hit the upper limit
        for (int i = 0; i < max; i++)
        {
            index++;
            index %= schedule.Count;
            timeTaken.Add(index);
            schedule.SetSchedule(index, ScheduleType.SLEEP);
        }
    }

    //populates a schedule with a given schedulleType given a max limit
    public void PopulateSchedule(int max, ScheduleType st, Schedule schedule)
    {
        //populate a random block and setting it to the schedule type
        for (int i = 0; i < max; i++)
        {
            int randNum = GetRandNum(scheduleMax);
            timeTaken.Add(randNum);
            schedule.SetSchedule(randNum, st);
        }
    }
}
