using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToDestory : MonoBehaviour
{
    public float time;
    private void OnEnable()
    {
        StartCoroutine(DestoryCorotine());
    }
    protected IEnumerator DestoryCorotine()
    {
        yield return new WaitForSeconds(time);
        GameEntry.ObjectPoolComponent.Release(gameObject);
    }
}
