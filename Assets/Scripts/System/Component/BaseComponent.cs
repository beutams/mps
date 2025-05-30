using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent<T> : SingletonNetBehaviour<T> where T : SingletonNetBehaviour<T>
{

}
