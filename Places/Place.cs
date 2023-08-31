using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//what the npc can do the most at this place i.e what "need" most of the tasks satisfy
//given that this was written earlier and the npc ai has changed, I might delete this
public enum PlaceCat { REST, EAT, PARTY, LEARN, WORK}
//type of place that dictates what jobs and tasks are here and what npcs can be here
public enum PlaceType { LIBRARY, HOUSE, STORE, SCHOOL, TAVERN, PARK, HOSPITAL, REC, GUARDTOWER, REST, GROCERY }

//a class not implemented yet
//for now it only houses basic information
//will be more useful later hopefully
public class Place : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField] private PlaceType placeType;
    [SerializeField] private Address address;

    //getters
    public Character Owner { get { return owner; } }
    public PlaceType GetPlaceType { get { return placeType; } }
    public Address GetAddress { get { return address; } }
}

// a cool idea in theory, but if I don't figure out how to make this work, I will get rid of it
[System.Serializable]
public class Address
{
    [SerializeField] string intersection;
    [SerializeField] int placeNumber;
    [SerializeField] string city;
    [SerializeField] string country;

    public string Intersection { get { return intersection; } }
    public int PlaceNumber { get { return placeNumber; } }
    public string City { get { return city; } }
    public string Country { get { return country; } }


    public Address(int num, string inter)
    {
        intersection = inter;
        placeNumber = num;
        city = "Elmyra";
        country = "Parsheba";
    }
}