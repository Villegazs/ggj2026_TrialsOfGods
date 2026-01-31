using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("States")]
    public MovementStateSO groundedStateSo;
    public MovementStateSO airStateSo;
    public MovementStateSO usingWindMaskStateSo;
    public MovementStateSO windMaskJumpStateSo;

    public PlayerStateMachine StateMachine { get; private set; }

    [Header("Default Movement")]
    public MovementSettingsSO defaultMovement;

    public float MoveSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float AirControl { get; private set; }
    public float Gravity { get; private set; }

    [Header("Jump")]
    public float jumpForce = 6f;
    [SerializeField] float groundedStickForce = -2f;
    
    [Header("Input")]
    [SerializeField] private GameInput gameInput;
    
    public float GroundedStickForce
    {
        get { return groundedStickForce; }
        set { groundedStickForce = value; }
    }
    [Header("Variable Jump")]
    [SerializeField] float jumpCutMultiplier = 0.4f;


    [Header("Jump Assist")]
    public float jumpBufferTime = 0.15f;
    public float coyoteTime = 0.1f;

    float jumpBufferTimer;
    float coyoteTimer;
    
    [Header("Air Jumps")]
    [SerializeField] int maxAirJumps = 1;

    int airJumpsRemaining;
    [Header("Mask")]
    [SerializeField] private Masks equippedMask = Masks.Light; // default

    


    public CharacterController Controller { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Vector3 HorizontalVelocity { get; set; }

    public Vector3 MoveInputWorld { get; private set; }
    public Vector3 MoveInputLocal { get; private set; }

    private bool equipMask = false;

    private PlayerInventory _playerInventory;
    
    // -------- ABILITIES --------
    Dictionary<AbilitySO, AbilityRuntimeData> abilityData =
        new Dictionary<AbilitySO, AbilityRuntimeData>();
    AbilitySO activeAbility;

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        StateMachine = new PlayerStateMachine();
        ApplyMovementSettings(defaultMovement);
    }

    void Start()
    {
        _playerInventory=this.gameObject.GetComponent<PlayerInventory>();
        airJumpsRemaining = maxAirJumps;
        StateMachine.Initialize(groundedStateSo, this);
    }

    void OnEnable()
    {
        StaticEventHandler.OnWindMaskActivated += ActivateWindMaskState;
        StaticEventHandler.OnMaskEquipped += EquipMaskAlternate;
    }

    void OnDisable()
    {
        StaticEventHandler.OnWindMaskActivated -= ActivateWindMaskState;
        StaticEventHandler.OnMaskEquipped -= EquipMaskAlternate;
    }

    void ActivateWindMaskState()
    {
        if (usingWindMaskStateSo != null)
        {
            StateMachine.SwitchState(usingWindMaskStateSo, this);
        }
        else
        {
            Debug.LogWarning("UsingWindMaskStateSO is not assigned in CharacterMovement!");
        }
    }

    void Update()
    {
        ReadJumpInput();
        UpdateTimers();

        // THIS runs every frame
        UpdateAbilities();

        StateMachine.Tick(this);
        activeAbility?.Tick(this);

        HandleJumpCut();
        ApplyMovement();
    }




    

    // ---------------- TIMERS ----------------

    void UpdateTimers()
    {
        if (JumpPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (Controller.isGrounded)
        {
            coyoteTimer = coyoteTime;
            airJumpsRemaining = maxAirJumps;
        }
        else
            coyoteTimer -= Time.deltaTime;
    }


    public bool HasBufferedJump() => jumpBufferTimer > 0f;
    public bool CanUseCoyoteJump() => coyoteTimer > 0f;
    public void ConsumeJumpBuffer() => jumpBufferTimer = 0f;

    // ---------------- MOVEMENT ----------------

    public void ApplyMovementSettings(MovementSettingsSO settings)
    {
        MoveSpeed = settings.moveSpeed;
        Acceleration = settings.acceleration;
        AirControl = settings.airControl;
        Gravity = settings.gravity;
    }

    public void HandleHorizontalMovement(float control)
    {
        //float x = Input.GetAxisRaw("Horizontal");
        //float z = Input.GetAxisRaw("Vertical");
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        MoveInputLocal = new Vector3(moveDir.x, 0f, moveDir.z);
        MoveInputLocal = Vector3.ClampMagnitude(MoveInputLocal, 1f);

        MoveInputWorld = transform.TransformDirection(MoveInputLocal);

        Vector3 desired = MoveInputWorld * MoveSpeed * speedMultiplier;

        HorizontalVelocity = Vector3.Lerp(
            HorizontalVelocity,
            desired,
            Acceleration * control * Time.deltaTime
        );
    }

    float speedMultiplier = 1f;

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }

    public void SetVerticalVelocity(float y)
    {
        Velocity = new Vector3(Velocity.x, y, Velocity.z);
    }

    public void AddVerticalVelocity(float y)
    {
        Velocity += Vector3.up * y;
    }

    void ApplyMovement()
    {
        Vector3 finalMove = HorizontalVelocity + Vector3.up * Velocity.y;
        Controller.Move(finalMove * Time.deltaTime);
    }
    // -------- INPUT CACHE --------
    public bool JumpPressedThisFrame { get; private set; }
    public bool JumpReleasedThisFrame { get; private set; }
    public bool JumpHeld { get; private set; }

    void ReadJumpInput()
    {
        JumpPressedThisFrame = Input.GetButtonDown("Jump");
        JumpReleasedThisFrame = Input.GetButtonUp("Jump");
        JumpHeld = Input.GetButton("Jump");
    }

    void HandleJumpCut()
    {
        if (JumpReleasedThisFrame && Velocity.y > 0f)
        {
            Velocity = new Vector3(
                Velocity.x,
                Velocity.y * jumpCutMultiplier,
                Velocity.z
            );
        }
    }



    public bool CanGroundJump()
    {
        return HasBufferedJump() && (Controller.isGrounded || CanUseCoyoteJump());
    }

    public bool CanAirJump()
    {
        return HasBufferedJump() && airJumpsRemaining > 0 && !Controller.isGrounded;
    }

    public void ConsumeAirJump()
    {
        airJumpsRemaining--;
    }

    // ---------------- ABILITIES ----------------

    AbilitySO[] allowedAbilities;
    public void SetAllowedAbilities(AbilitySO[] abilities)
    {
        allowedAbilities = abilities;
    }

    public void UpdateAbilities()
    {
        if (allowedAbilities == null || allowedAbilities.Length == 0)
            return;

        AbilitySO chosen = null;

        foreach (var ability in allowedAbilities)
        {
            if (!abilityData.TryGetValue(ability, out var data))
                abilityData[ability] = data = new AbilityRuntimeData();

            if (data.cooldownTimer > 0f)
                data.cooldownTimer -= Time.deltaTime;

            if (ability.WantsToActivate(this))
                data.inputBufferTimer = ability.inputBufferTime;
            else
                data.inputBufferTimer -= Time.deltaTime;

            if (data.HasBufferedInput &&
                !data.IsOnCooldown &&
                ability.CanUse(this))
            {
                if (chosen == null || ability.priority > chosen.priority)
                    chosen = ability;
            }
        }

        if (chosen != null)
            ExecuteAbility(chosen);

        if (activeAbility != null &&
            activeAbility.activationType == AbilitySO.AbilityActivationType.Hold &&
            !activeAbility.WantsToActivate(this))
        {
            activeAbility.End(this);
            activeAbility = null;
        }
    }

    private GameObject _maskPickup;
    
    public void SetMaskPickup(GameObject maskPrefab)
    {
        _maskPickup = maskPrefab;
    }
    
    
    public void EquipMask(Masks mask)
    {
        if (_maskPickup)
        {  
            equippedMask = mask;
        }
        else
        {
            print("Falta mascara pelotudo");
        }
    }

    public void EquipMaskPrefab()
    {
        _playerInventory.EquipMask(_maskPickup);
    }

    public void RemoveMask()
    {
        _playerInventory.RemoveMask();
    }

    private void EquipMaskAlternate()
    {
        equipMask = !equipMask;
    }

    public bool CanActivateWindMask()
    {
        return equippedMask == Masks.Wind && usingWindMaskStateSo != null && equipMask;
    }


    void ExecuteAbility(AbilitySO ability)
    {
        if (activeAbility == ability)
            return;

        // Priority guard
        if (activeAbility != null &&
            ability.priority < activeAbility.priority)
            return;

        if (activeAbility != null)
            activeAbility.End(this);

        activeAbility = ability;

        var data = abilityData[ability];
        data.inputBufferTimer = 0f;
        data.cooldownTimer = ability.cooldown;

        ability.Execute(this);
    }
    public void EndAbility(AbilitySO ability)
    {
        if (activeAbility != ability)
            return;

        ability.End(this);
        activeAbility = null;
    }



}
