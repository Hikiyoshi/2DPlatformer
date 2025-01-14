using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : State
{
    public Navigate navigate;
    public IdleState idle;
    public Transform anchor1;
    public Transform anchor2;

    void GoToNextDestination()
    {
        //Random Spot for patrolling
        // float randomSpot = Random.Range(anchor1.position.x, anchor2.position.x);
        // navigate.destination = new Vector2(randomSpot, core.transform.position.y);
        
        //Go to 2 point for patrolling
        if(navigate.destination == (Vector2)anchor1.position)
        {
            navigate.destination = (Vector2)anchor2.position;
        }
        else
        {
            navigate.destination = (Vector2)anchor1.position;
        }

        Set(navigate, true);
    }

    public override void Enter()
    {
        GoToNextDestination();
    }

    public override void Do()
    {
        if(state == navigate)
        {
            if(navigate.isComplete)
            {
                Set(idle, true);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            if(machine.state.time > 1)
            {
                GoToNextDestination();
            }
        }
    }
}
