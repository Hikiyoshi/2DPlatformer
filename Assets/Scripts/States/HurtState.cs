using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HurtState : State
{
    public AnimationClip anim;
    
    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = 0.8f;
        if(CheckAnimationCompleted(anim))
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }

    public void ResetIsComplete()
    {
        isComplete = false;
    }
}
