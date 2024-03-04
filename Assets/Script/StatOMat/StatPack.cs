using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class StatPack
{
    public StatPack()
    {

    }

    public StatPack(string name, float value)
    {
        statName = name;
        statValue = value;
    }

    public string statName;


    public float statValue;

}
