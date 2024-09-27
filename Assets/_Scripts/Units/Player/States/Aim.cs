using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : PlayerState
{
    private float weightLayer = 0;
    private readonly float t = 8;

    public Aim(PlayerController.PlayerState stateKey) : base(stateKey)
    {
        _moveSpeed *= 0.75f;
        _sprintSpeed = _moveSpeed;

        LayerNameAnim = "Aiming";
        LayerAnimID = PlayerController.Instance.PlayerAnim.GetLayerIndex(LayerNameAnim);
    }

    public override void EnterState()
    {
        base.EnterState();

        NextState = PlayerController.PlayerState.Aiming;
    }
    public override void UpdateState()
    {
        CalculateWeight();
        PlayerController.Instance.PlayerAnim.SetLayerWeight(LayerAnimID, weightLayer);

        if ((InputManager.Instance.IsSprint || !InputManager.Instance.IsAim) && weightLayer <= 0)
            NextState = PlayerController.PlayerState.Combat;
        else if (InputManager.Instance.IsAttack && weightLayer >= 1)
            NextState = PlayerController.PlayerState.Attacking;
        else
        {
            CalculateTargerSpeed();
            CalculateTargerAngle();
            CalculateLookAngle();

            HandleMovement();
            HandleRotation(_mouseRotation);
            HandleAnimation();
        }
    }

    private void CalculateWeight()
    {
        int isAim = InputManager.Instance.IsAim ? 1 : -1;

        if (isAim == 1 && weightLayer > 1) weightLayer = 1;
        else if (isAim == -1 && weightLayer < 0) weightLayer = 0;
        else weightLayer += isAim * t * Time.deltaTime;
    }
}
