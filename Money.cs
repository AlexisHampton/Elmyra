using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//currency for the game
public enum MoneyType { SAPPHIRE, OPAL, ONYX}


/// <summary>
/// Class <c>Money</c> o --> onyx, p-> opal, s-> sapphire
/// holds the money for all characters and items 
/// </summary>
[Serializable]
public class Money
{
    [SerializeField] private int amount;
    [SerializeField] private MoneyType moneyType;

    //getters
    public int Amount { get { return amount; } }
    public MoneyType GetMoneyType {  get { return moneyType; } }

    //constructor that takes a string of [amt][char] --> 2p
    public Money(string s)
    {
        moneyType = SetMoneyType(s[s.Length - 1]); //set the money type to the last character
        amount = int.Parse(s.Substring(0, s.Length-1).ToString());//everything else is the amt
    }

    //constructor that simplifies everything
    public Money(int amt, MoneyType mt)
    {
        amount = amt;
        moneyType = mt;
    }

    //sets money type depending on the char passed 
    public MoneyType SetMoneyType(char s)
    {
        MoneyType mt = MoneyType.SAPPHIRE;
        if (s == 'p')
            mt = MoneyType.OPAL;
        else if (s == 'o')
            mt = MoneyType.ONYX;
        return mt;
    }

    //increase money
    //decrease money
    //convert 100s to 1p and 100p to 1o
}
