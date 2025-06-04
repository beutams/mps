using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent<T> : SingletonMonoBehaviour<T> where T : SingletonMonoBehaviour<T>
{

}
