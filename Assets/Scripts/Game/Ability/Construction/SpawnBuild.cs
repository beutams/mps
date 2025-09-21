using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "SpawnBuild", menuName = "ScriptableObject/Construction/SpawnBuild")]
public class SpawnBuild : Ability
{
    public override void Init(GameObjectController owner)
    {
        base.Init(owner);
        owner.AddComponent<SpawnBuildMono>();
    }
}
