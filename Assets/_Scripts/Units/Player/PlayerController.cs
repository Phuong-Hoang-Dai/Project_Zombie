using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StateManager<PlayerController.PlayerState>
{
    public static PlayerController Instance { get; private set; }
    public GameObject Player { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator PlayerAnim { get; private set; }

    public AudioClip attackAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip[] FootstepAudioClips;

    [SerializeField]
    protected float interactRange = 2f;
    public IInteractable InteractableObj { get; private set; }

    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    protected override void Awake()
    {
        if (Instance != null) Debug.LogError("Only player allow to exists");
        Instance = this;
    }

    protected override void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        PlayerAnim = GetComponent<Animator>();
        Player = gameObject;

        states.Add(PlayerState.Normal, new NormalState(PlayerState.Normal));
        states.Add(PlayerState.Combat, new NormalInCombat(PlayerState.Combat));
        states.Add(PlayerState.Aiming, new Aim(PlayerState.Aiming));
        states.Add(PlayerState.Attacking, new AttackingState(PlayerState.Attacking));
        currentState = states[0];

        base.Start();
    }

    protected override void Update()
    {
        base.Update();


        UpdateInteractalbeObject();
        if (InputManager.Instance.IsInteract && InteractableObj != null) InteractableObj.Interact();
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index],
                    transform.TransformPoint(CharacterController.center), FootstepAudioVolume);
            }
        }
    }

    private void UpdateInteractalbeObject()
    {
        InteractableObj = null;

        //Get interactable objects in range
        List<IInteractable> interactableObjects = new();
        Collider[] objectInRange = new Collider[5];
        Physics.OverlapSphereNonAlloc(transform.position, interactRange, objectInRange);
        foreach (Collider collider in objectInRange)
            if (collider.TryGetComponent(out IInteractable interactableObject))
                interactableObjects.Add(interactableObject);

        //Get the interactable object closest
        foreach (IInteractable obj in interactableObjects)
        {
            if (InteractableObj == null) InteractableObj = obj;
            else if (Vector3.Distance(transform.position, InteractableObj.GetTransform().position)
            > Vector3.Distance(transform.position, obj.GetTransform().position))
            {
                InteractableObj = obj;
            }
        }
    }

    public Vector2 GetPlayerPosition() => new(transform.position.x, transform.position.z);


    public enum PlayerState
    {
        Normal,
        Combat,
        Aiming,
        Attacking
    }
}
