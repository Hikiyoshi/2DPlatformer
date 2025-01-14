using System;
using Unity.Mathematics;
using UnityEngine;

public class RunState : State
{
    public AnimationClip anim;
    
    public float maxXSpeed = 2.5f;
    
    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        float velX = rb.velocity.x;
        
        animator.speed = Helpers.Map(maxXSpeed, 0, 1, 0, 1.6f, true); 
        
        if(!core.surfaceSensor.grounded || Mathf.Abs(velX) < 0.1f){
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}