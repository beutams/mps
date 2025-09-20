using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBuild : Ability
{
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        owner.AddComponent<SpawnBuildMono>();
    }
}
