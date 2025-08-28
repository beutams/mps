using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapItem : MonoBehaviour
{
    public void Locate(Vector3 position)
    {
        transform.position = new Vector3(position.x, 90, position.z);
    }
}
