using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : Core
{
    public IdleState idleState;
    public RunState runState;
    public AirState airState;
    public CrouchState crouchState;
    public NormalAttack attack;
    public Dash dash;
    public Slide slide;
    public WallSlide wallSlide;
    public ClimbLadder climb;
    public HurtState hurtState;
    public DeathState deathState;

    public Damageable damageable;

    public float acceleration = 0.29f;

    [Range(0, 1)]
    public float groundDecay = 0.9f;

    //public float maxXSpeed = 2.5f;
    //public float jumpSpeed = 3.5f;
    
    public float xInput {get; private set;}
    public float yInput {get; private set;}

    private bool canJump = true;

    public bool isWallSliding {get; private set;}

    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpCounter;

    public bool isClimbing {get; private set;}

    //Variables for attack
    private bool canAttack = true;
    public bool isAttacking {get; private set;}

    //Variables for dash
    private bool canDash = true;
    public bool isDashing {get; private set;}

    //Variables for slide
    private bool canSlide = true;
    public bool isSliding {get; private set;}

    public bool isHurt {get; private set;} = false;

    private void Start()
    {
        SetupInstances();

        stateMachine.Set(idleState);

        //state = idleState;
    }

    private void Update()
    {
        if(deathState.isComplete)
        {
            Application.Quit();
            EditorApplication.isPlaying = false;
            return;
        }

        CheckInput();

        HandleHurt();

        HandleJump();

        HandleWallSlide();

        HandleAttack();

        HandleDash();

        HandleSlide();

        HandleWallJump();

        checkClimbLadder();

        SelectState();

        //state.Do();

        stateMachine.state.Do();
    }

    private void FixedUpdate()
    {
        //CheckGround();

        ApplyFiction();

        HandleXMovement();

        HandleClimbLadder();
    }

    private void SelectState(){
        //State oldState = state;

        if(damageable != null)
        {
            if(!damageable.isAlive)
            {
                stateMachine.Set(deathState);
                return;
            }

            if(isHurt)
            {
                stateMachine.Set(hurtState);
                return;
            }
        }

        if(isDashing){
            stateMachine.Set(dash);
            return;
        }

        if(isSliding){
            stateMachine.Set(slide);
            return;
        }

        if(isAttacking){
            stateMachine.Set(attack);
            return;
        }

        if(surfaceSensor.grounded){

            if(yInput < 0 && Mathf.Abs(xInput) < 0.1f){
                //state = crouchState;
                
                stateMachine.Set(crouchState);
            }
            else if(xInput == 0){
                //state = idleState;

                stateMachine.Set(idleState);
            }
            else if(Mathf.Abs(xInput) >=0.1 && !isDashing && !isSliding){
                //state = runState;
                
                stateMachine.Set(runState);
            }
        }
        else if(isClimbing){
            stateMachine.Set(climb);
        }
        else if(isWallSliding && !isWallJumping){
            stateMachine.Set(wallSlide);
        }
        else{
            //state = airState;

            stateMachine.Set(airState);
        }

        /*
        if(oldState != state || oldState.isComplete)
        {
            oldState.Exit();
            state.Initialize();
            state.Enter();
        }
        */
    }

    //Get Input from keyboard by arrow or w,a,s,d
    private void CheckInput()
    {
        if(!isWallJumping)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }
        else
        {
            xInput = 0;
            yInput = 0;
        }
    }

    private void HandleXMovement()
    {
        if(Mathf.Abs(xInput) > 0 && !isAttacking && !isWallJumping && !isHurt && damageable.isAlive)
        {
            //increment velocity by our acceleration, then clamp within max
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(rb.velocity.x + increment, -runState.maxXSpeed, runState.maxXSpeed);
            rb.velocity = new Vector2(newSpeed, rb.velocity.y);

            if(!isWallSliding)
            {
                FaceInput();
            }
        }
        // Vector2 direction = new Vector2(xInput, yInput).normalized;
        // rb.velocity = direction * speed;
    }
    private void FaceInput()
    {
        float direction = Mathf.Sign(xInput);

        transform.localScale = new Vector3(direction, 1, 1);
    }

    private void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && surfaceSensor.grounded && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, airState.jumpSpeed);
        }
    }

    private void HandleWallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallSlide.wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallSlide.wallJumpPower.x, wallSlide.wallJumpPower.y);
            wallJumpCounter = 0;

            if(transform.localScale.x != wallJumpDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallSlide.wallJumpDuration);
        }

    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void HandleWallSlide()
    {
        if(Mathf.Abs(xInput) > 0 && surfaceSensor.isWalled && !surfaceSensor.grounded)
        {
            isWallSliding = true;

            float fallSpeed = Mathf.Clamp(rb.velocity.y, -wallSlide.wallSlidingSpeed, float.MaxValue);
            rb.velocity = new Vector2(rb.velocity.x, fallSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void checkClimbLadder()
    {
        if(Mathf.Abs(yInput) > 0 && surfaceSensor.isLadder)
        {
            isClimbing = true;
        }

        if(!surfaceSensor.isLadder || xInput == 0 && yInput == 0 && surfaceSensor.grounded)
        {
            isClimbing = false;
        }
    }
    private void HandleClimbLadder()
    {
        if(isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(xInput, yInput * climb.speedClimbing);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void HandleAttack()
    {
        if(isAttacking)
        {
            if(attack.isComplete)
            {
                isAttacking = false;
                canJump = true;
                attack.ResetIsComplete();
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && surfaceSensor.grounded && canAttack)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isAttacking = true;
            canJump = false;
        }
    }
    private void StopAttack()
    {
        isAttacking = false;
    }

    private void HandleDash()
    {
        if(Input.GetKeyDown(KeyCode.X) && surfaceSensor.grounded && canDash)
        {
            if(isAttacking)
            {
                StopAttack();
            }
            StartCoroutine(Dash());
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        canJump = false;
        canSlide = false;

        isDashing = true;

        //float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0f;
        rb.AddForce(new Vector2(transform.localScale.x * dash.dashPower, 0f), ForceMode2D.Impulse);
        //rb.velocity = new Vector2(transform.localScale.x * dash.dashPower, 0f);

        yield return new WaitForSeconds(dash.dashingTime);

        //rb.gravityScale = originalGravity;
        canJump = true;
        canSlide = true;
        isDashing = false;

        yield return new WaitForSeconds(dash.dashingCooldown);

        canDash = true;
    }

    private void HandleSlide()
    {
        if(Input.GetKeyDown(KeyCode.Z) && surfaceSensor.grounded && canSlide)
        {
            if(isAttacking)
            {
                StopAttack();
            }
            StartCoroutine(Slide());
        }
    }
    private IEnumerator Slide()
    {
        canSlide = false;
        canJump = false;
        canAttack = false;
        canDash = false;

        isSliding = true;

        //float originalGravity = rb.gravityScale;
        //rb.gravityScale = 0f;
        rb.AddForce(new Vector2(transform.localScale.x * slide.slidePower, 0f), ForceMode2D.Impulse);
        //rb.velocity = new Vector2(transform.localScale.x * slide.slidePower, 0f);

        yield return new WaitForSeconds(slide.slideTime);

        //rb.gravityScale = originalGravity;

        canJump = true;
        canAttack = true;
        canDash = true;

        isSliding = false;

        yield return new WaitForSeconds(slide.slideCooldown);

        canSlide = true;
    }

    private void HandleHurt()
    {
        if(isHurt && hurtState.isComplete)
        {
            isHurt = false;
            hurtState.ResetIsComplete();
        }
    }
    public void GotHit(int damage, Vector2 knockBack)
    {
        isHurt = true;
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
        xInput = -MathF.Sign(knockBack.x) * 0.1f;

        if(!isWallSliding)
        {
            FaceInput();
        }
    }
    
    private void ApplyFiction()
    {
        if(surfaceSensor.grounded && xInput == 0 && rb.velocity.y <= 0){
            rb.velocity *= groundDecay;
        }
    }
}
