using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalInCombat : PlayerState
{
    public NormalInCombat(PlayerController.PlayerState stateKey) : base(stateKey)
    {
        _sprintSpeed *= 1.126f;
        _moveSpeed *= 1.5f;

        LayerNameAnim = "Combat with meele weapon";
        LayerAnimID = PlayerController.Instance.PlayerAnim.GetLayerIndex(LayerNameAnim);

        PlayerController.Instance.PlayerAnim.SetLayerWeight(LayerAnimID, 1);
    }

    public override void EnterState()
    {
        base.EnterState();

        NextState = PlayerController.PlayerState.Combat;
    }

    public override void UpdateState()
    {

        if (InputManager.Instance.IsAim && !InputManager.Instance.IsSprint) 
            NextState = PlayerController.PlayerState.Aiming;

        else base.UpdateState();
    }
}
