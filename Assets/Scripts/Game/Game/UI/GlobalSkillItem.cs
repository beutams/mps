using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSkillItem : MonoBehaviour
{
    protected Transform mask;
    protected Transform skill;
    protected CoverAbility ability;

    protected float progress = 0;

    private void Update()
    {
        progress = ability.GetProgress();
        if(progress > 0)
        {

        }
    }
    public void DoSkill(GameObjectController obj, Vector3 targetPosition)
    {
        if (ability.CanDo())
            ability.Do();
        else if (ability.CanDo(obj))
            ability.Do(obj);
        else if (ability.CanDo(targetPosition))
            ability.Do(targetPosition);
    }
    public void Init(CoverAbility ability)
    {
        this.ability = ability;
    }
}
