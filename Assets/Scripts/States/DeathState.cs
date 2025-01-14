using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    public AnimationClip anim;
    
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
