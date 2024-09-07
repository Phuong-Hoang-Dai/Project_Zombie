using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, IStateContext
{
    private static GameObject _player;
    public static GameObject Player => _player;
    private static PlayerController instance;
    public static PlayerController Instance => instance;

    // Audio
    public AudioClip attackAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    // player
    private float _rotationSmoothTime = 0.12f;
    private float _moveSpeed = 2.0f;
    private float _sprintSpeed = 5.335f;
    private float _speedOffset = 0.1f;
    private float _speedChangeRate = 20.0f;

    private float _rotation;
    private float _speed;
    private float _animationSpeed_X;
    private float _animationSpeed_Y;

    public float RotationSmoothTime => _rotationSmoothTime;
    public float SprintSpeed => _sprintSpeed;
    public float MoveSpeed => _moveSpeed;
    public float SpeedOffset => _speedOffset;
    public float SpeedChangeRate => _speedChangeRate;

    // animation IDs
    private int _animIDSpeed_X;
    private int _animIDSpeed_Y;
    private int _animIDMotionSpeed;

    private CharacterController player;
    private Animator playerAnim;
    private Weapon weapon;
    public Camera cam;

    private PlayerState state;
    public PlayerState State { get => state; }

    private void Awake()
    {
        if (Player != null) Debug.LogError("Only player allow to exists");
        _player = gameObject;

        if (Instance != null) Debug.LogError("Only playerController allow to exists");
        instance = this;

        state = new NormalState(this);
    }

    private void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();

        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed_X = Animator.StringToHash("Speed_X");
        _animIDSpeed_Y = Animator.StringToHash("Speed_Y");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void Update()
    {
        weapon.Attack();

        state.UpdateState(playerAnim);

        CalculateTargerSpeed();
        CalculateAngleSpeed();
        HandleRotation();
        HandleMovement();
        HandleAnimation();
    }

    private void CalculateTargerSpeed() => state.CalculateTargerSpeed();
    private void CalculateAngleSpeed() => state.CalculateTargerAngle();

    private void HandleRotation()
    {
        state.CalculateRotation(ref _rotation);

        player.transform.rotation = Quaternion.Euler(0.0f, _rotation, 0.0f);
    }

    private void HandleMovement()
    {
        state.CalculateMovement(ref _speed, out Vector3 moveDirection);

        player.Move(_speed * moveDirection * Time.deltaTime);
    }

    private void HandleAnimation()
    {
        state.CalculateAnimationBlend(ref _animationSpeed_X, ref _animationSpeed_Y);

        playerAnim.SetFloat(_animIDSpeed_X, _animationSpeed_X);
        playerAnim.SetFloat(_animIDSpeed_Y, _animationSpeed_Y);
        playerAnim.SetFloat(_animIDMotionSpeed, 1f);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index],
                    transform.TransformPoint(player.center), FootstepAudioVolume);
            }
        }
    }

    public void StartAttack() => weapon.StartAttack();
    public void StopAttack() => weapon.StopAttack();

    public Vector2 GetPlayerPosition() => new Vector2(transform.position.x, transform.position.z);
    public void ChangeState(PlayerState newState) => state = newState;
}
