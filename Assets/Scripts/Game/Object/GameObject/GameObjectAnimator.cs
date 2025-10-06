using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectAnimator : MonoBehaviour
{
    protected Animator animator;
    public virtual void Play(string name)
    {
        animator?.Play(name);
    }
    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
}
