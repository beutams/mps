using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArmoryData
{
    public int hero;
    public int unit;
    public int aircraft;
    public int scout;
    public List<string> globalSkills;
    public ArmoryData() 
    {
        hero = 1;
        unit = 1;
        aircraft = 1;
        globalSkills = new List<string>() { "FireSupportAbility", "ParadropAbility", "PrecisionBombingAbility" };
    }
}