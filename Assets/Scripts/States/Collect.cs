using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : State
{
    //Check for any souls in the vicinity.
    public List<Transform> souls;

    public Transform target;
    
    //if you found one, run over it.
    public Navigate navigate;

    //idle for two second afterward, then complete.
    public IdleState idle;

    public float collectRadius = 0.1f;

    public float vision = 1;

    public override void Enter()
    {
        navigate.destination = target.position;
        Set(navigate, true);
    }

    public override void Do()
    {
        if(state == navigate)
        {
            if(CloseEnough(target.position)) 
            {
                Set(idle, true);
                rb.velocity = new Vector2(0, rb.velocity.y);
                target.gameObject.SetActive(false);
                return;
            }
            else if(!InVision(target.position))
            {
                Set(idle, true);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                navigate.destination = target.position;
                Set(navigate, true);
            }
        }
        else
        {
            if(state.time > 2)
            {
                isComplete = true;
            }
        }
    }

    public override void Exit()
    {
        
    }

    public bool CloseEnough(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < collectRadius;
    }

    public bool InVision(Vector2 targetPos)
    {
        return Vector2.Distance(core.transform.position, targetPos) < vision;
    }

    public void CheckForTarget()
    {
        foreach(Transform soul in souls)
        {
            if(InVision(soul.position) && soul.gameObject.activeSelf)
            {
                target = soul;
                return;
            }
        }

        target = null;
    }
}
