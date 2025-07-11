using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDCompnent : MonoBehaviour, ID
{
    public int id;
    public string searchName;
    public int ID => id;
}
