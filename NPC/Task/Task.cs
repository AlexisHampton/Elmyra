using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the broadest way an npc can interact with the world.
//This class stores all the information needed for a task at this point in time
public class Task : MonoBehaviour
{
    [SerializeField] private string taskName;
    [SerializeField] private TaskSO taskSO;
    [SerializeField] int duration; //in schedule blocks
    [SerializeField] int energyDepleted;
    [SerializeField] int energyRestored;
    [SerializeField] Transform location;
    [SerializeField] List<ReqSkill> reqSkills;
    [SerializeField] List<TraitSO> traits;

    //getters
    public string TaskName { get { return taskName; } }
    public Need TaskNeed { get { return taskSO.need; } }
    public int Duration { get { return duration; } }
    public int EnergyDepleted { get { return energyDepleted; } }
    public int EnergyRestored { get { return energyRestored; } }
    public Transform Location { get { return location; } }
    public List<ReqSkill> ReqSkills { get { return reqSkills; } }
    public Animation Anim { get { return taskSO.anim; } }
    public List<TraitSO> Traits { get { return traits; } }

    private NPC npc;

    //sets the location of the task before the game starts
    private void Awake()
    {
        location = gameObject.transform;
    }

    //returns the task score depending on how far away the task is from the npc
    //how much energy is will give and how long it taskes [in schedule blocks]
    public float GetTaskScore(Vector3 pos)
    {
        float distance = (pos - location.position).magnitude;
        return energyRestored - energyDepleted - distance - duration;
    }

    //returns the energy gained
    public int GetEnergy()
    {
        return energyRestored - energyDepleted;
    }

    //returns if an npc can be set and sets them 
    public bool SetNPC(NPC n)
    {
        if (HasNPC()) return false;

        npc = n;
        return true;
    }

    //removes an npc from the task object if they are there
    public void RemoveNPC()
    {
        if (HasNPC())
            npc = null;
    }

    //is there an npc?
    public bool HasNPC()
    {
        return npc != null;
    }

    //checks whether the alloted time blocks have passed
    public bool IsDone(int d)
    {
        return duration <= d;
    }
}
