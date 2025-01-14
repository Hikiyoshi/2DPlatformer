using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : State
{
    public AnimationClip anim;

    public PlayerMovement input;

    public float speedClimbing = 1.5f;

    public override void Enter()
    {
        animator.Play(anim.name);
    }

    public override void Do()
    {
        if(input.yInput == 0)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = Helpers.Map(speedClimbing, 0, 1, 0 , 0.8f, true);
        }

        if(!input.isClimbing)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        
    }
}
