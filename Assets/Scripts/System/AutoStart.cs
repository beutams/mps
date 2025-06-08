using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStart : MonoBehaviour
{
    public GameObject obj;
    void Update()
    {
        if (obj.GetComponent<SingleSubUI>())
        {
            obj.GetComponent<SingleSubUI>().OnEnterClick();
            Destroy(this);
        }
    }
}
