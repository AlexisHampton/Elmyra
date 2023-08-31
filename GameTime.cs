using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//controls the time for the entire game
public class GameTime : MonoBehaviour
{
    public static GameTime Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timeText;
    //ingame minutes until next time increase
    [SerializeField] private int minutesToNextTime = 30; 

    //how the schedule of an npc is broken up, one is the length of minutesToNextTime
    public int ScheduleBlocks = 48;
    //{ get { return 24 * (60 / MinutesToNextTime); } }


    public int MinutesToNextTime { get { return minutesToNextTime; } }

    public event EventHandler<OnIncreaseTimeEventArgs> OnIncreaseTime;
    public class OnIncreaseTimeEventArgs : EventArgs
    {
        public Clock gameTime;
    }

    private Clock gameTime = new Clock();
    private float timer; //keeps track of how far away we are from a time increase

    //initialization of the timer and timeText
    private void Awake()
    {
        Instance = this;
        timer = minutesToNextTime;
        timeText.text = gameTime.ToString();
    }

    //every frame, decrease timer and increase the time when it's 0. 
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameTime.IncreaseTimeMin(minutesToNextTime);
            timeText.text = gameTime.ToString();
            //send out an event to all subscribers
            OnIncreaseTime?.Invoke(this, new OnIncreaseTimeEventArgs { gameTime = gameTime });
            timer = minutesToNextTime; //reset timer
        }
    }
}

//a simple clock class that provides a backend for our game time
[System.Serializable]
public class Clock : IComparable<Clock>
{
    public int hour;
    public int minutes;

    //init hour and min to 0
    public Clock()
    {
        hour = minutes = 0;
    }

    //sets the hour and min to specified parameters
    public Clock(int h, int m)
    {
        hour = h;
        minutes = m;
    }

    //sets this clock's hours and minutes to another clock's hours and minutes
    public Clock(Clock c)
    {
        hour = c.hour;
        minutes = c.minutes;
    }

    //returns the same clock but but increased by the min perscribed
    public Clock IncreaseTimeMin(int min)
    {
        minutes += min;
        hour += minutes / 60;
        minutes %= 60;
        return this;
    }

    //returns the same clock but increased by the hours perscribed, leaves minutes untouched
    public Clock IncreaseTimeHR(int hrs)
    {
        hour += hrs;
        return this;
    }

    //turns a clock into something easily printable
    public override string ToString()
    {
        string h = "";
        string min = minutes + "";
        if (minutes < 10)
            min += "0";
        if (hour < 10)
            h += "0";
        return h + hour +":" + min;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Clock) return false;
        Clock other = (Clock)obj;
        if (hour != other.hour) return false;
        if (minutes != other.minutes) return false;
        return true;
    }

    //1 if greater, 0 is equal, -1 if less than
    public int CompareTo(Clock other)
    {
        if (hour > other.hour)
            return 1;
        else if (hour == other.hour)
        {
            if (minutes > other.minutes) return 1;
            else if (minutes == other.minutes) return 0;
            else return -1;
        }
        return -1;
    }

    public static bool operator <(Clock a, Clock b)
    {
        if (a.CompareTo(b) == -1) return true;
        return false;
    }
    public static bool operator >(Clock a, Clock b)
    {
        if (a.CompareTo(b) == 1) return true;
        return false;
    }
    public static bool operator ==(Clock a, Clock b)
    {
        return a.CompareTo(b) == 0;
    }

    public static bool operator !=(Clock a, Clock b)
    {
        return a.CompareTo(b) != 0;
    }
}

//a class with two clocks a start and end time, used in store hours or job hours
[System.Serializable]
public class Hours
{
    public Clock startTime;
    public Clock endTime;
}