using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : StateManager<PlayerController.PlayerState>
{
    public static PlayerController Instance { get; private set; }
    public GameObject Player { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Animator PlayerAnim { get; private set; }
    public PlayerStats Stats { get; private set; }
    public Weapon Weapon { get; private set; }

    public Weapon[] WeaponList { get; private set; }

    public AudioClip attackAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip[] FootstepAudioClips;

    [SerializeField]
    protected float interactRange = 2f;
    public IInteractable InteractableObj { get; private set; }

    protected float fadeSpeed = 0.5f;
    public Image DeadBackground;
    public bool isDead = false;

    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    protected override void Awake()
    {
        if (Instance != null) Debug.LogError("Only player allow to exists");
        Instance = this;

        CharacterController = GetComponent<CharacterController>();
        PlayerAnim = GetComponent<Animator>();
        Stats = GetComponent<PlayerStats>();

        WeaponList = GetComponentsInChildren<Weapon>(true);
    }

    protected override void Start()
    {

        Player = gameObject;

        //states.Add(PlayerState.Normal, new NormalState(PlayerState.Normal));
        states.Add(PlayerState.Combat, new NormalInCombat(PlayerState.Combat));
        states.Add(PlayerState.Aiming, new Aim(PlayerState.Aiming));
        states.Add(PlayerState.Attacking, new AttackingState(PlayerState.Attacking));
        currentState = states[PlayerState.Combat];

        base.Start();
    }

    protected override void Update()
    {
        if(!isDead) base.Update();

        Dead();

        UpdateInteractalbeObject();
        if (InputManager.Instance.IsInteract && InteractableObj != null) InteractableObj.Interact();
    }

    public void Dead()
    {
        if(Stats.CurrentHp <= 0)
        {
            isDead = true;

            PlayerAnim.SetTrigger("isDead");

            DeadBackground.gameObject.SetActive(true);

            var tempColor = DeadBackground.color;
            tempColor.a += fadeSpeed * Time.deltaTime;
            if (tempColor.a > 1) tempColor.a = 1;
            DeadBackground.color = tempColor;
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
            if(collider != null)
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
 

    public void Attack(IDamageable enemy)
    {
        if (Weapon == null)
            return;

        if(enemy == null) return;

        enemy.TakeDamage(Stats.BaseAtk);
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

    public void Equip(Item item)
    {
        if (Weapon != null) Weapon.gameObject.SetActive(false);
        if (item == InventoryManager.Instance.emptyItem || item == null) return;

        foreach (var weapon in WeaponList)
        {
            if (weapon.gameObject.name == item.prefab.name)
            {
                Weapon = weapon;
                Weapon.gameObject.SetActive(true);
                return;
            }
        }
        Weapon = null;
    }

    public void UpdateStats(Item.StatToChange stat, float amoutChange )
    {
        if(stat == Item.StatToChange.Hp) Stats.CurrentHp += amoutChange;
        if(stat == Item.StatToChange.MaxHp) Stats.MaxHp += amoutChange;
        if(stat == Item.StatToChange.Def) Stats.Def += amoutChange;
        if(stat == Item.StatToChange.Atk) Stats.BaseAtk += amoutChange;
    }


    public void StartAttack()
    {
        return;
    }
    public void StopAttack()
    {
        return;
    }

    public Vector2 GetPlayerPosition() => new(transform.position.x, transform.position.z);

    public enum PlayerState
    {
        Normal,
        Combat,
        Aiming,
        Attacking,
        Dead
    }
}