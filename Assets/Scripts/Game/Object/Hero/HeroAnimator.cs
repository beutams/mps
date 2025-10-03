using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    public Animator animator;
    protected HeroController controller;
    void Start()
    {
        controller = GetComponent<HeroController>();
    }
    void Update()
    {
        animator.SetFloat("Velocity", controller.curVelocity);
        animator.SetFloat("Rotation", controller.curTrun);
    }
}
