using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ArmoryData : UserData
{
    public int hero = -1;
    public List<int> globalSkills;
    public ArmoryData() 
    {
        hero = 1;
        globalSkills = new List<int>() {1,2,3 };
    }
    public override string ToString()
    {
        return $"{hero},{globalSkills[0]},{globalSkills[1]},{globalSkills[2]}";
    }
}