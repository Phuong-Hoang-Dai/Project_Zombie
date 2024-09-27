using System;
using System.Collections;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerInput.PlayerControlActions _playerCotrolActions;

    private PlayerInput.UIActions _UIActions;
    private PlayerInput _playerInput;

    [field: SerializeField]
    public int ControlMenu { get; private set; }
    [field: SerializeField]
    public bool IsSprint { get; private set; }
    [field: SerializeField]
    public bool IsAttack { get; private set; }
    [field: SerializeField]
    public bool IsAim { get; private set; }
    [field: SerializeField]
    public bool IsInteract { get; private set; }
    [field: SerializeField]
    public bool IsInteractUI { get; private set; }
    [field: SerializeField]
    public bool IsInventoryOpen { get; private set; }
    [field: SerializeField]
    public bool IsCloseUI { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public Vector2 MousePositionOnScreen { get; private set; }
    private Vector2 centerPositionOnScreen = new Vector3(Screen.width / 2, Screen.height / 2);

    public bool isCombat = false;
    public Camera MainCamera;

    protected override void Awake()
    {
        base.Awake();

        _playerInput = new PlayerInput();

        _playerCotrolActions = _playerInput.PlayerControl;
        _UIActions = _playerInput.UI;

        _playerCotrolActions.Sprint.started += OnSprint;
        _playerCotrolActions.Sprint.canceled += OnCancelSprint;

        _playerCotrolActions.Aim.started += OnAim;
        _playerCotrolActions.Aim.canceled += OnCancelAim;

        _playerCotrolActions.Attack.started += OnAttack;
        _playerCotrolActions.Attack.canceled += OnCancelAttack;

        _playerCotrolActions.Interact.started += OnInteract;
        _playerCotrolActions.Interact.canceled += OnCancelInteract;

        _UIActions.Interact.started += OnInteractUI;
        _UIActions.Interact.canceled += OnCancelInteractUI;

        _playerCotrolActions.Move.started += OnMove;
        _playerCotrolActions.Move.performed += OnMove;
        _playerCotrolActions.Move.canceled += OnMove;

        _playerCotrolActions.Look.started += OnLook;
        _playerCotrolActions.Look.performed += OnLook;
        _playerCotrolActions.Look.canceled += OnLook;

        _UIActions.ControlMenu.started += OnControlMenu;
        _UIActions.ControlMenu.performed += OnControlMenu;
        _UIActions.ControlMenu.canceled += OnControlMenu;

        _playerCotrolActions.OpenInventory.started += OnOpenInventory;
        _playerCotrolActions.OpenInventory.canceled += OnCancelOpenInventory;

        _UIActions.CloseUI.started += OnCloseUI;
        _UIActions.CloseUI.canceled += OnCancelCloseUI;
    }

    private void Update()
    {
        UpdateMousePositionOnScree();
        UpdateMousePosition();
    }

    private void UpdateMousePositionOnScree() => MousePositionOnScreen = Input.mousePosition;

    private void UpdateMousePosition()
    {
        if (IsMouseInCenter()) return;

        Ray ray = MainCamera.ScreenPointToRay(MousePositionOnScreen);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        MousePosition = new Vector2(hit.point.x, hit.point.z);
    }

    public Vector2 GetLookDirection()
    {
        if (LookInput != Vector2.zero)
        {
            StartCoroutine(CenterCursor());
            return Quaternion.Euler(0, 0, -45) * LookInput;
        }
        else if (!IsMouseInCenter())
        {
            return Quaternion.Euler(0, 0, -10) * (MousePosition - PlayerController.Instance.GetPlayerPosition());
        }
        return new Vector2(PlayerController.Instance.Player.transform.forward.x, 
            PlayerController.Instance.Player.transform.forward.z);
    }

    private bool IsMouseInCenter() => Vector2.Distance(MousePositionOnScreen, centerPositionOnScreen) < 2f;

    IEnumerator CenterCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        yield return null;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnMove(InputAction.CallbackContext context) => MoveInput = _playerCotrolActions.Move.ReadValue<Vector2>();
    private void OnLook(InputAction.CallbackContext context) => LookInput = _playerCotrolActions.Look.ReadValue<Vector2>();
    private void OnControlMenu(InputAction.CallbackContext context) => 
        ControlMenu = Mathf.RoundToInt(_UIActions.ControlMenu.ReadValue<float>());

    public void OnAim(InputAction.CallbackContext context) => IsAim = true;
    public void OnCancelAim(InputAction.CallbackContext context) => IsAim = false;

    public void OnSprint(InputAction.CallbackContext context) => IsSprint = true;
    public void OnCancelSprint(InputAction.CallbackContext context) => IsSprint = false;

    public void OnAttack(InputAction.CallbackContext context) => IsAttack = true;
    public void OnCancelAttack(InputAction.CallbackContext context) => IsAttack = false;

    public void OnInteract(InputAction.CallbackContext context) => IsInteract = true;
    public void OnCancelInteract(InputAction.CallbackContext context) => IsInteract = false;

    public void OnInteractUI(InputAction.CallbackContext context) => IsInteractUI = true;
    public void OnCancelInteractUI(InputAction.CallbackContext context) => IsInteractUI = false;

    public void OnOpenInventory(InputAction.CallbackContext context) => IsInventoryOpen = true;
    public void OnCancelOpenInventory(InputAction.CallbackContext context) => IsInventoryOpen = false;

    public void OnCloseUI(InputAction.CallbackContext context) => IsCloseUI = true;
    public void OnCancelCloseUI(InputAction.CallbackContext context) => IsCloseUI = false;

    public void DeactiveControlPlayer() => _playerInput.PlayerControl.Disable();
    public void ActiveControlPlayer() => _playerInput.PlayerControl.Enable();

    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }
}
