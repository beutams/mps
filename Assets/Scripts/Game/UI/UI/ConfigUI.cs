using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigUI : UIBase
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }
}
