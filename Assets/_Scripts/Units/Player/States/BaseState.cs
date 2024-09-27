using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public EState StateKey { get; protected set; }
    public EState NextState { get; protected set; }

    public BaseState(EState stateKey)
    {
        StateKey = stateKey;
        NextState = stateKey;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    public virtual void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerExit(Collider collider) { }
}
