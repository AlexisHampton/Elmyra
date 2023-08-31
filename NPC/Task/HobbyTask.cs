using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tasks that will be used when the npc is scheduled for hobby time
public class HobbyTask : Task
{
    [SerializeField] Need need = Need.FUN;

    public Need GetNeed { get { return need; } }
}
