using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set;}

    protected float startTime;

    public float time => Time.time - startTime;

    protected Core core;

    protected Rigidbody2D rb => core.rb;
    protected Animator animator => core.animator;

    public StateMachine machine;
    protected StateMachine parent;
    public State state => machine.state;

    public virtual void Enter() {}

    public virtual void Do() {}

    public virtual void FixedDo() {}

    public virtual void Exit() {}

    /*
    public void Setup(Rigidbody2D _body, Animator _animator, PlayerMovement _playerMovement)
    {
        this.rb = _body;
        this.animator = _animator;
        this.input = _playerMovement;
        isComplete = false;
    }
    */

    public void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset);
    }
    public void SetCore(Core _core)
    {
        machine = new StateMachine();
        core = _core;
    }

    public void DoBranch()
    {
        Do();
        state?.DoBranch();
    }
    public void FixedDoBranch()
    {
        FixedDo();
        state?.FixedDoBranch();
    }

    public bool CheckAnimationCompleted(AnimationClip anim)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        return stateInfo.IsName(anim.name) && stateInfo.normalizedTime >= 1;
    }

    public void Initialize(StateMachine _parent)
    {
        parent = _parent;
        isComplete = false;
        startTime = Time.time;
    }
}