using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : PlayerState
{
    public NormalState(PlayerController.PlayerState stateKey) : base(stateKey)
    {
    }
    public override void UpdateState()
    {
        if (true) NextState = PlayerController.PlayerState.Combat;
        //else base.UpdateState();
    }
}
