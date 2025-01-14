using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : State
{
    public AnimationClip anim;

    public int damage;
    public Vector2 knockBack = Vector2.zero;

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

    private void OnTriggerEnter2D(Collider2D other) {
        Damageable damageable = other.GetComponentInParent<Damageable>();

        if(damageable != null)
        {
            Vector2 knockBackDirection = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            
            damageable.Hit(damage, knockBackDirection);
        }
    }

    public void ResetIsComplete()
    {
        isComplete = false;
    }
}
