using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDComponent : MonoBehaviour, ID
{
    protected int id = 0;
    protected IDType idType = IDType.None;
    public int ID => id;

    public IDType searchName => idType;

}
