using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArmoryData : UserData
{
    public int hero;
    public List<int> globalSkills;
    public ArmoryData() 
    {
        hero = 0;
        globalSkills = new List<int>() { };
    }
}