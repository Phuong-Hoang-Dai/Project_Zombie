using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> states = new();

    protected BaseState<EState> currentState;

    protected bool isTransiting;

    protected abstract void Awake();

    protected virtual void Start() => currentState.EnterState();

    protected virtual void Update()
    {
        if (!isTransiting && currentState.StateKey.Equals(currentState.NextState))
        {
            currentState.UpdateState();
        }
        else if (!isTransiting) TransitionToState(currentState.NextState);
    }

    public virtual void TransitionToState(EState stateKey)
    {
        isTransiting = true;
        currentState.ExitState();
        currentState = states[stateKey];
        currentState.EnterState();
        isTransiting = false;
    }

    protected virtual void OnTriggerEnter(Collider collider) 
        => currentState.OnTriggerEnter(collider);
    protected virtual void OnTriggerStay(Collider collider) 
        => currentState.OnTriggerStay(collider);
    protected virtual void OnTriggerExit(Collider collider) 
        => currentState.OnTriggerExit(collider);
}
