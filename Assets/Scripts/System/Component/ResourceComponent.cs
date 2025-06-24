using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceComponent : BaseComponent<ResourceComponent>
{
    public T GetResource<T>(string name) where T : class
    {
        return null;
    }
    public Sprite GetImage(string path)
    {
        return null;
    }
}
