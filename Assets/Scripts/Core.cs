using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class Core is for State Machine. When entities use State, they have to inherit this class 
public abstract class Core : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    //public PlayerMovement input;

    public SurfaceSensor surfaceSensor;

    public StateMachine stateMachine;

    public State state => stateMachine.state;

    protected void Set(State newState, bool forceReset = false)
    {
        stateMachine.Set(newState, forceReset);
    }

    public void SetupInstances()
    {
        stateMachine = new StateMachine();

        State[] allChildStates = GetComponentsInChildren<State>();

        foreach(State state in allChildStates)
        {
            state.SetCore(this);
        }
    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        if(Application.isPlaying && state != null)
        {
            List<State> states = stateMachine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States: " + string.Join(" > ", states));
        }
#endif
    }
}
