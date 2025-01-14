using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : State
{
    public Vector2 destination;
    public float speed = 1f;
    public float threshold = 0.1f;
    public State stateUse;

    public override void Enter()
    {
        Set(stateUse, true);
    }

    public override void Do()
    {
        if(Vector2.Distance(core.transform.position,destination) < threshold)
        {
            isComplete = true;
        }
        FaceDestination();
    }

    public override void FixedDo()
    {
        Vector2 direction = (destination - (Vector2)core.transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
    }

    private void FaceDestination()
    {
        core.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
    }
}
