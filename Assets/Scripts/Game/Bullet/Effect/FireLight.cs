using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FireLight : BulletEffect
{
    public string effectName;
    public float time;
    protected override void OnBulletStart()
    {
        base.OnBulletStart();
        GameObject obj = GameEntry.ObjectPoolComponent.Get(effectName);
        obj.transform.position = transform.position;
        obj.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        StartCoroutine(DestoryCorotine(obj));
    }
    protected IEnumerator DestoryCorotine(GameObject obj)
    {
        yield return new WaitForSeconds(time);
        GameEntry.ObjectPoolComponent.Release(obj);
    }
}
