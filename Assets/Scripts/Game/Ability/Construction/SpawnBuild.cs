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
        Vector3 point = owner.transform.position + new Vector3(0, 5, 0);
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(point);
        GameObject obj = GameEntry.ObjectPoolComponent.Get("SpawnBuildIcon");
        obj.transform.SetParent(GameObject.Find("SpawnBuildCanvas").transform);
        obj.GetComponent<RectTransform>().position = screenPoint;
    }
}
