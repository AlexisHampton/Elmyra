using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Need { EAT, HYGIENE, SOCIAL, REST, FUN, NULL }

//handles all the needs for a given character and how full they are
[System.Serializable]
public class CharNeed
{
    public Need need;
    //how full the need is
    public int priority; //100 is full, 0-100

    //constructor
    public CharNeed(int n, int p)
    {
        need = (Need)n;
        priority = p;
    }

    //increases the priority of the need
    public void IncreasePriority(int p)
    {
        priority += p;
        priority = Mathf.Clamp(priority, 0, 100);
    }
}

//handles all the bits of a character from their houses to their energy to their needs to their money,
//everything that makes them a person
public class Character : MonoBehaviour
{

    [Header("Character Attributes")]
    [SerializeField] protected CharNeed[] charNeeds = new CharNeed[5];
    [SerializeField] protected int energy = 70; //0-100
    [SerializeField] protected House house;
    [SerializeField] protected JobSO jobSO; //base job, I might get rid of this, only for testing
    [SerializeField] protected Job job;
    [SerializeField] protected Money money;
    [SerializeField] protected List<Skill> skills;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactionMask;

    //getters
    public CharNeed[] CharNeeds { get { return charNeeds; } }
    public int Energy { get { return energy; } }
    public House GetHouse { get { return house; } }
    public Job GetJob { get { return job; } }
    public Money GetMoney { get { return money; } }
    public List<Skill> Skills { get { return skills; } }


    //inits all charNeeds with a random value
    private void Awake()
    {

        for (int i = 0; i < 5; i++)
            charNeeds[i] = new CharNeed(i, Random.Range(80, 91));
       
    }

    //interacts with the something if close enough and it's interactable
    public void HandleInteractions()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactDistance, interactionMask);
        foreach(Collider collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                interactable.Interact();
        }
    }

    //returns the need with the lowest priority
    public Need GetLowestNeed()
    {
        CharNeed lowest = charNeeds[0];
        foreach (CharNeed charNeed in charNeeds)
            if (lowest.priority > charNeed.priority)
                lowest = charNeed;
        return lowest.need;
    }

    //increases a priority for a specific need 
    public void IncreasePriorityForNeed(Need need)
    {
        foreach (CharNeed charNeed in charNeeds)
            if (charNeed.need == need)
                charNeed.priority += NPCManager.Instance.NeedPriorityIncrease;
    }

    //decreases a priority for a specific need 
    public void DecreasePriorityForNeed(Need need)
    {
        foreach (CharNeed charNeed in charNeeds)
            if (charNeed.need == need)
                charNeed.priority -= NPCManager.Instance.NeedPriorityIncrease;
    }

    //increases a priority for all needs
    public void IncreasePriorityForAllNeeds()
    {
        foreach (CharNeed charNeed in charNeeds)
            charNeed.priority += NPCManager.Instance.NeedPriorityIncrease;
    }

    //decreases a priority for all needs
    public void DecreasePriorityForAllNeeds()
    {
        foreach (CharNeed charNeed in charNeeds)
            charNeed.priority -= NPCManager.Instance.NeedPriorityIncrease;
    }
    
    //sets the current energy level for the character
    public void SetEnergy(int newEnergy)
    {
        energy += newEnergy;
        if (energy <= 0)
        {
            //disease maybe
            energy = 0;
        }
        else if (energy >= 100)
            energy = 100;
    }


    //I think I'll get rid of this later, but haven't tested player interaction yet
    public void IsCollisionDetected()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, GameManager.Instance.MaxCollisionDistance, GameManager.Instance.CharCollisionLayerMask))
            if (hit.distance <= 0.2)
                CanInteract(hit.collider);
    }

    //have not implemented this function yet, might not 
    public void CanInteract(Collider other)
    {

        

        if (other.GetType() == typeof(NPC))
        {
            //potentially speak
        }
        else if (other.GetType() == typeof(Player))
        {
            //potentially speak
        }
        else if (other.GetType() == typeof(Gatherable))
        {
            //gather, add to inventory 
        }
        else if (other.GetType() == typeof(Item))
        {
            Task task = other.GetComponent<Item>().GetTask();
           

        }
    }
}
