using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : State
{
    public AnimationClip anim;

    public float dashPower = 8f;
    public float dashingTime = 0.7f;
    public float dashingCooldown = 2f;

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

    public override void Exit()
    {
        
    }
}
