using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : State
{
    public AnimationClip anim;

    public PlayerMovement input;

    public float wallSlidingSpeed = 2f;
    public float wallJumpTime = 0.2f;
    public float wallJumpDuration = 0.4f;
    public Vector2 wallJumpPower = new Vector2(1.5f, 3f);

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        animator.speed = 1;
        
        if(core.surfaceSensor.grounded || !core.surfaceSensor.isWalled || input.xInput == 0)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}
