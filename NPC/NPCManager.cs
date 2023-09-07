using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages everything npc!
//used for big things that all npcs need and will later handle the npcs in different cities!
public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }
    [SerializeField] private List<NPC> allNPCs;
    [SerializeField] private Schedule schedule; //an empty schedule
    [SerializeField] private int needPriorityIncrease = 5; //the increase for increasing needs


    public int NeedPriorityIncrease { get { return needPriorityIncrease; } }

    private void Awake()
    {
        Instance = this;
    }

    //creates an empty schedule and subscribes to GameTime's OnIncreaseTime event
    private void Start()
    {
        schedule = new Schedule(GameTime.Instance.MinutesToNextTime, GameTime.Instance.ScheduleBlocks);
        GameTime.Instance.OnIncreaseTime += DoTask;
    }

    //every schedule block increase, go to the next shcedule block for every npc
    private void DoTask(object sender, GameTime.OnIncreaseTimeEventArgs e)
    {
        int index = schedule.GetIndex(e.gameTime);
        foreach (NPC npc in allNPCs)
        {
            npc.GoToTask(index);
        }
    }

    //initialize all npcs, only run at start
    public void InitNPCs()
    {
        foreach (NPC npc in allNPCs)
        {
            //make them random
            npc.InitTraits();
            npc.InitSkills();
            npc.ObtainJob();
            npc.SetSchedule();

        }
    }
}
