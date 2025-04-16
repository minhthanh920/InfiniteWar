using DG.Tweening.Core.Easing;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : BaseManager<Player>
{
    public Volume postProcessVolume;
    [SerializeField]
    private PlayerSO m_PlayerSO;
    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private Vector2 userInput;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private float m_Heath;
    private float m_Speed;
    private float m_Stanima;
    private float m_MeleeDamage;
    private float m_RangedDamage;


    private float jumpHeight;
    private float gravity;
    private float stepDown;
    private float airControl;
    private float jumpDamp;
    private float groundSpeed;
    private float pushPower;
    private bool isJumping;
    private int isSprintingParam = Animator.StringToHash("IsSprinting");
    private StarterAssetsInputs m_Input;

    public float m_AttackTime;

    void Start()
    {

        
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_Input = GetComponent<StarterAssetsInputs>();
        if (DataManager.HasInstance)
        {
            jumpHeight = DataManager.Instance.GlobalConfig.jumpHeight;
            gravity = DataManager.Instance.GlobalConfig.gravity;
            stepDown = DataManager.Instance.GlobalConfig.stepDown;
            airControl = DataManager.Instance.GlobalConfig.airControl;
            jumpDamp = DataManager.Instance.GlobalConfig.jumpDamp;
            groundSpeed = DataManager.Instance.GlobalConfig.groundSpeed;
            pushPower = DataManager.Instance.GlobalConfig.pushPower;
        }
        SetupDefault();
    }

    void Update()
    {
        SetDeath();
        if(IsPlayerDeath())
        {
            return;
        }
        if (m_AttackTime > 0)
        {
            m_AttackTime -= Time.deltaTime;
        }
        userInput.x = Input.GetAxis("Horizontal");
        userInput.y = Input.GetAxis("Vertical");

        m_Animator.SetFloat("x", userInput.x);
        m_Animator.SetFloat("y", userInput.y);

        //UpdateIsSprinting();
        UpdateAnimation();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            if(damage - m_Heath > 0)
            {
                Debug.Log($"Player Nhan Damage : {damage}");
                m_Heath -= damage;
            }
            else
            {
                SetDeath();
            }
        }
    }
    private void SetupDefault()
    {
        if (m_PlayerSO != null)
        {
            m_Heath = m_PlayerSO.m_Heath;
            m_Speed = m_PlayerSO.m_Speed;
            m_Stanima = m_PlayerSO.m_Stamina;
            m_MeleeDamage = m_PlayerSO.m_MeleeDamage;
            m_RangedDamage = m_PlayerSO.m_RangeDamage;
        }
        else
        {
            Debug.Log("PlayerSO is null");
        }
    }
    private void SetDeath()
    {
        if (m_Heath <= 0)
        {
            m_Animator.SetBool("IsDeath", true);
        }
    }
    private bool IsPlayerDeath()
    { 
        return m_Heath < 0f; 
    }
    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }
    private void UpdateAnimation()
    {
        m_Animator.SetBool("Grounded", m_CharacterController.isGrounded);
        if(!m_CharacterController.isGrounded)
        {
            return;
        }
        if (m_Input.m_PlayerAction == PlayerStateID.None)
        {
            m_Animator.SetBool("Idle", true);
        }
        else
        {
            m_Animator.SetBool("Idle", false);
        }
        if (m_Input.m_PlayerAction == PlayerStateID.Jump)
        {
            //m_Animator.SetBool("IsJumping", m_Input.jump);
            //m_Input.jump = false;
            //m_Input.m_PlayerAction = PlayerAction.None;
            //m_Animator.SetBool("FreeFall", m_Input.jump);
        }
        //else
        //{
        //    m_Animator.SetBool("IsJumping", false);
        //}
        if(m_Input.attack && m_CharacterController.isGrounded)
        {
            m_Animator.Play("Attack_3Combo_1");
            m_Input.attack = false;
        }
        //if (m_Input.m_PlayerAction == PlayerAction.Attack)
        //{
        //    m_Animator.Play("Attack_3Combo_1");
        //    m_Animator.SetBool("Idle", true);
        //    m_Input.attack = false;
        //    m_Input.m_PlayerAction = PlayerAction.None;
        //    //m_Animator.SetBool("Attack", true);
        //}
        //else
        //{
        //
        //}
        if (m_Input.m_PlayerAction == PlayerStateID.Walk)
        {
            m_Animator.SetBool("Walk", true);
        }
        else
        {
            m_Animator.SetBool("Walk", false);
        }
        if (m_Input.m_PlayerAction == PlayerStateID.Run)
        {
            m_Animator.SetBool("Run", true);
        }
        else
        {
            m_Animator.SetBool("Run", false);
        }
        if(userInput == Vector2.zero)
        {
            m_Input.m_PlayerAction = PlayerStateID.None;
        }
    }

    //private bool IsSprinting()
    //{
    //    bool isSprinting = Input.GetKey(KeyCode.LeftShift);
    //    //bool isFiring = activeWeapon.IsFiring();
    //    //bool isReloading = reloadWeapon.isReloading;
    //    //bool isChangingWeapon = activeWeapon.isChangingWeapon;
    //    //bool isAiming = characterAiming.isAiming;
    //    //return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    //}

    //private void UpdateIsSprinting()
    //{
    //    bool isSprinting = IsSprinting();
    //    animator.SetBool(isSprintingParam, isSprinting);
    //    rigController.SetBool(isSprintingParam, isSprinting);
    //    if (userInput.x != 0)
    //    {
    //        if (postProcessVolume.profile.TryGet(out ChromaticAberration chromaticAberration))
    //        {
    //            chromaticAberration.active = isSprinting;
    //        }
    //    }
    //}

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        m_CharacterController.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;
        
        if (!m_CharacterController.isGrounded)
        {
            SetInAir(0);
        }
    }
    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 airDisplacement = velocity * Time.fixedDeltaTime;
        airDisplacement += CalculateAircontrol();
        //Debug.Log($"magnitude : {airDisplacement.magnitude}");
        m_CharacterController.Move(airDisplacement);
        isJumping = !m_CharacterController.isGrounded;
        rootMotion = Vector3.zero;
        m_Animator.SetBool("IsJumping", isJumping);

    }

    private void OnAnimatorMove()
    {
        rootMotion += m_Animator.deltaPosition;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = m_Animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        m_Animator.SetBool("Idle", false);
        m_Animator.SetBool("Walk", false);
        m_Animator.SetBool("Run", false);
        m_Animator.SetBool("IsJumping", true);
    }

    private Vector3 CalculateAircontrol()
    {
        return ((transform.forward * userInput.y) + (transform.right * userInput.x)) * (airControl / 100);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3f)
            return;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushPower;
    }

    //public void OnFootStep()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_FOOTSTEP);
    //    }
    //}

    //public void OnJump()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
    //    }
    //}
    public void OnAttack()
    {

    }
}