using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tasks that will be used when the npc is scheduled for need time
public class NeedTask : Task
{
    [SerializeField] private Need need;

    public Need GetNeed { get { return need; } }

}
