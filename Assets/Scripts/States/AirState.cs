using System;
using UnityEngine;

public class AirState : State
{
    public AnimationClip anim;
    
    public float jumpSpeed = 3.5f;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        //seek the animation to the frame based on our y velocity
        //the time map the y velocity with 0 and 1 
        float time = Helpers.Map(rb.velocity.y, jumpSpeed, -jumpSpeed, 0, 1, true);

        animator.Play(anim.name, 0, time);
        animator.speed = 0;

        if(core.surfaceSensor.grounded){
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}