using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSensor : MonoBehaviour
{
    public bool grounded {get; private set;}
    public bool isWalled {get; private set;}
    public bool isLadder {get; private set;}

    public BoxCollider2D groundCheck;
    public BoxCollider2D bodyCol;
    public Transform wallCheck;

    public LayerMask groundMask;
    public LayerMask wallMask;
    public LayerMask ladderMask;

    private void FixedUpdate() {
        CheckGround();
        CheckWall();
        CheckLadder();
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void CheckWall()
    {
        isWalled = Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallMask);
    }

    void CheckLadder()
    {
        isLadder = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, ladderMask).Length > 0 || Physics2D.OverlapAreaAll(bodyCol.bounds.min, bodyCol.bounds.max, ladderMask).Length > 0;
    }
}
