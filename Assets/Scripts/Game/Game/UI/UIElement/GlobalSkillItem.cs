using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSkillItem : MonoBehaviour
{
    protected Transform mask;
    protected Transform skill;
    protected GlobalSkillData data;

    protected float progress = 0;

    private void Update()
    {
        progress = data.ability.GetProgress();
        if(progress > 0)
        {

        }
    }
    public void DoSkill(GameObjectController obj, Vector3 targetPosition)
    {
        if (data.ability.CanDo())
            data.ability.Do();
        else if (data.ability.CanDo(obj))
            data.ability.Do(obj);
        else if (data.ability.CanDo(targetPosition))
            data.ability.Do(targetPosition);
    }
    public void Init(GlobalSkillData data)
    {
        this.data = data;
        data.ability = Instantiate(data.ability);
    }
}
[CreateAssetMenu(fileName = "AutoSpawn", menuName = "ScriptableObject/Universal/GlobalSkill")]
public class GlobalSkillData
{
    public string imgPath;
    public CoverAbility ability;
}
