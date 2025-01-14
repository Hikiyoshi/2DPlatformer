using System;
using UnityEngine;

public class IdleState : State
{
    public AnimationClip anim;
    
    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = 1;
        if(!core.surfaceSensor.grounded){
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}