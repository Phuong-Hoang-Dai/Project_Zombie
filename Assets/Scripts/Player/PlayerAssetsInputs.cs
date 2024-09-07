using System;
using System.Collections;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAssetsInputs : MonoBehaviour
{
    private static PlayerAssetsInputs instance;
    public static PlayerAssetsInputs Instance => instance;

    private InputAction _moveAction;
    private InputAction _sprintAction;
    private InputAction _aimAction;
    private InputAction _attackAction;
    private InputAction _lookAction;
    private InputManager inputManager;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprint;
    private bool isAim;
    private bool isAttack;
    private Vector2 mousePosition;
    private Vector2 mousePositionOnScreen;
    private Vector2 centerPositionOnScreen = new Vector3(Screen.width / 2, Screen.height / 2);

    public Camera MainCamera;
    public bool isCombat = false;

    private void Awake()
    {
        if (instance != null) Debug.LogError("Only 1 PlayerAssetsInputs allow to exists");
        instance = this;

        inputManager = new InputManager();

        _moveAction = inputManager.PlayerControl.Move;
        _sprintAction = inputManager.PlayerControl.Sprint;
        _aimAction = inputManager.PlayerControl.Aim;
        _attackAction = inputManager.PlayerControl.Attack;
        _lookAction = inputManager.PlayerControl.Look;

        _sprintAction.started += onSprint;
        _sprintAction.canceled += onCancelSprint;

        _aimAction.started += onAim;
        _aimAction.canceled += onCancelAim;

        _attackAction.started += onAttack;
        _attackAction.canceled += onCancelAttack;

        _moveAction.started += onMove;
        _moveAction.performed += onMove;
        _moveAction.canceled += onMove;

        _lookAction.started += onLook;
        _lookAction.performed += onLook;
        _lookAction.canceled += onLook;
    }

    private void Update()
    {
        UpdateMousePositionOnScree();
        UpdateMousePosition();
    }

    private void UpdateMousePositionOnScree() => mousePositionOnScreen = Input.mousePosition;

    private void UpdateMousePosition()
    {
        if (IsMouseInCenter()) return;

        Ray ray = MainCamera.ScreenPointToRay(mousePositionOnScreen);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        mousePosition = new Vector2(hit.point.x, hit.point.z);
    }

    private bool IsMouseInCenter() => Vector2.Distance(mousePositionOnScreen, centerPositionOnScreen) < 2f;

    public void onAim(InputAction.CallbackContext context) => isAim = true;
    public void onCancelAim(InputAction.CallbackContext context) => isAim = false;

    public void onSprint(InputAction.CallbackContext context) => isSprint = true;
    public void onCancelSprint(InputAction.CallbackContext context) => isSprint = false;

    public void onAttack(InputAction.CallbackContext context) => isAttack = true;
    public void onCancelAttack(InputAction.CallbackContext context) => isAttack = false;

    private void onMove(InputAction.CallbackContext context) => moveInput = _moveAction.ReadValue<Vector2>();
    private void onLook(InputAction.CallbackContext context) => lookInput = _lookAction.ReadValue<Vector2>();

    public bool IsSprint() => isSprint;
    public bool IsAiming() => isAim;
    public bool IsCombat() => isCombat;
    public bool IsAttack() => isAttack;
    public Vector2 GetMoveInput() => moveInput;
    public Vector2 GetMousePositionOnScreen() => mousePosition;
    public Vector2 GetLookDirection()
    {
        if (lookInput != Vector2.zero)
        {
            //StartCoroutine(CenterCursor());
            return Quaternion.Euler(0, 0, -45) * lookInput;
        } 
        else if(!IsMouseInCenter())
        {
            return Quaternion.Euler(0, 0, -10) * (mousePosition - PlayerController.Instance.GetPlayerPosition());
        }
        return new Vector2(PlayerController.Instance.transform.forward.x, PlayerController.Instance.transform.forward.z);
    }

    private void OnEnable() { inputManager.Enable(); }
    private void OnDisable() { inputManager.Disable(); }

    IEnumerator CenterCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }
}
