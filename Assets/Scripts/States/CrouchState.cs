using System;
using UnityEngine;

public class CrouchState : State
{
    public AnimationClip anim;
    
    public PlayerMovement input;
    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = 1;
        if(!core.surfaceSensor.grounded || input.yInput >= 0 || input.xInput != 0){
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}