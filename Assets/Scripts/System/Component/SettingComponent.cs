using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingComponent : BaseComponent
{
    [Header("Camera")]
    public float CameraMoveSpeed = 6f;
    [Header("QuadTree")]
    public int maxDepth = 5;
    public int maxObject = 5;
}
