using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateScriptableObject : ScriptableObject
{
    [MenuItem("Create/Ability")]
    public static void CreateAbility()
    {
        CreateInstance("Ability");
    }
}
