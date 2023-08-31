using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a class that holds the basics of an item
//not really implemented
//might even be abstract in the future with 2 children, gatherable and taskItem or sthg?
//everything in this file will likely change
public class Item : MonoBehaviour
{
    [SerializeField] private Task task;

    public Task GetTask()
    {
        return task;
    }
}
