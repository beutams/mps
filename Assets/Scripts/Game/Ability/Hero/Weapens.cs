using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapens", menuName = "ScriptableObject/Unit/Weapens")]
public class Weapens : Ability
{
    public override bool CanDo(Vector3 target)
    {
        return InputManager.instance.GetFire();
    }
    public override bool CanDo(GameObjectController target)
    {
        return InputManager.instance.GetFire();
    }
    public override void Do(Vector3 target)
    {
        base.Do(target);
        HeroController heroController = owner as HeroController;
        foreach(var item in heroController.weapenGroup[heroController.GetCurrentGroup()])
        {
            item.weapen.Fire(null, target,item);
        }
    }
    public override void Do(GameObjectController target)
    {
        base.Do(target);
        HeroController heroController = owner as HeroController;
        foreach (var item in heroController.weapenGroup[heroController.GetCurrentGroup()])
        {
            item.weapen.Fire(target, target.transform.position, item);
        }
    }
}