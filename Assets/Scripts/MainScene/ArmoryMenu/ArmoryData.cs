using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArmoryData
{
    public int hero;
    public List<int> globalSkills;
    public ArmoryData() 
    {
        hero = 0;
        globalSkills = new List<int>() { };
    }
}