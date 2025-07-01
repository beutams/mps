using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStart : MonoBehaviour
{
    public GameObject obj;
    public ArmorySubUI armory;
    void Update()
    {
        armory.SetData(ArmorySubUI.ArmoryType.Hero, 0);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 0);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 1);
        armory.SetData(ArmorySubUI.ArmoryType.GlobalSkillsAdd, 2);
        if (obj.GetComponent<SingleSubUI>())
        {
            obj.GetComponent<SingleSubUI>().OnEnterClick();
            Destroy(this);
        }
    }
}
