using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : State
{
    public AnimationClip anim;

    public float slidePower = 8f;
    public float slideTime = 0.5f;
    public float slideCooldown = 0.5f;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = 1;
        
        if(CheckAnimationCompleted(anim))
        {
            isComplete = true;
        }
    }

    public override void FixedDo()
    {
        
    }

    public override void Exit()
    {
        
    }
}
