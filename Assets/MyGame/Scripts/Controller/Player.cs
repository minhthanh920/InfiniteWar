using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public Volume postProcessVolume;
    public Player m_Player;
    public Animator m_Animator;
    public Vector2 m_UserInput;
    public AnimatorStateInfo m_StateInfo;
    public PlayerWeapon m_Weapon;
    public ParticleSystem m_EffectPrefab;
    [SerializeField]
    private PlayerSO m_PlayerSO;
    public CharacterController m_CharacterController;

    private string m_GameOver = "GameOver";
    private Vector3 m_RootMotion;
    private Vector3 m_Velocity;
    private float m_Heath;
    private float m_Speed;
    private float m_Mana;
    private float m_MaxHeath;
    private float m_MaxSpeed;
    private float m_MaxMana;
    private float m_Stanima;
    private float m_MaxStanima;
    private float m_MeleeDamage;
    private float m_RangedDamage;
    private float m_AttackTime = 1f;
    private CharacterStateID CurrentStateID = CharacterStateID.Idle;


    private float m_JumpHeight;
    private float m_Gravity;
    private float stepDown;
    private float airControl;
    private float jumpDamp;
    private float groundSpeed;
    private float pushPower;
    private bool isJumping;
    private bool m_IsDead;
    public StarterAssetsInputs m_Input;
    private PlayerStateMachine m_StateMachine;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<PlayerStateMachine>();
        m_Input = GetComponent<StarterAssetsInputs>();
        m_Weapon = GetComponentInChildren<PlayerWeapon>();

    }
    void Start()
    {
        SetupDefault();
        if(GameManager.HasInstance)
        {
            GameManager.Instance.SetPlayer(this);
        }    
        
    }
    void Update()
    {

        if (IsPlayerDeath())
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }

        //if(UIManager.Instance.GetExistPopup<PopupSetting>())
        //{
        //    return;
        //}
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GameManager.Instance.PauseGame();
        //}
        m_UserInput.x = Input.GetAxis("Horizontal");
        m_UserInput.y = Input.GetAxis("Vertical");
        UpdateState();
        ShowPopup();
        //UpdateIsSprinting();

    }
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            m_Heath -= damage;
            Debug.Log($"m_Heath : {m_Heath}");
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, this);
            if (m_Heath <= 0)
            {
                UIManager.Instance.ShowPopup<PopupPlayerDead>();
                SetDeath();
            }
        }
    }
    private void ShowPopup()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup<PopupPlayerInfomation>();
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup<PopupPauseGame>();
            }
            return;
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPopup<PopupCheatGame>();
            }
            return;
        }
    }
    public void SetupDefault()
    {
        if (DataManager.HasInstance)
        {
            m_JumpHeight = DataManager.Instance.GlobalConfig.jumpHeight;
            m_Gravity = DataManager.Instance.GlobalConfig.gravity;
            stepDown = DataManager.Instance.GlobalConfig.stepDown;
            airControl = DataManager.Instance.GlobalConfig.airControl;
            jumpDamp = DataManager.Instance.GlobalConfig.jumpDamp;
            groundSpeed = DataManager.Instance.GlobalConfig.groundSpeed;
            pushPower = DataManager.Instance.GlobalConfig.pushPower;
        }
        if (m_PlayerSO != null)
        {
            m_MaxHeath = m_PlayerSO.m_Heath;
            m_MaxSpeed = m_PlayerSO.m_Speed;
            m_MaxStanima = m_PlayerSO.m_Stamina;
            m_MaxMana = m_PlayerSO.m_Mana;
            m_Heath = m_MaxHeath;
            m_Speed = m_MaxSpeed;
            m_Stanima = m_MaxStanima;
            m_Mana = m_MaxMana;
            m_MeleeDamage = m_PlayerSO.m_MeleeDamage;
            m_RangedDamage = m_PlayerSO.m_RangeDamage;
            m_IsDead = false;
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_USER_INFO, this);
        }
        else
        {
            Debug.Log("PlayerSO is null");
        }
    }
    private void UpdateState()
    {
        if (m_Heath <= 0f)
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }

        // Kiểm tra trạng thái tấn công
        if (m_Input.attack && m_CharacterController.isGrounded)
        {
            // Nếu nhân vật chưa vào trạng thái tấn công, vào trạng thái Attack
            if (m_StateMachine.m_CurrentStateID != CharacterStateID.Attack)
            {
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
            return;
        }

        // Kiểm tra animation tấn công đã hoàn thành chưa
        //if (m_StateMachine.m_CurrentStateID == CharacterStateID.Attack)
        //{
        //    AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        //    if (stateInfo.normalizedTime >= 1f)  // Animation đã hoàn thành
        //    {
        //        // Sau khi hoàn thành animation Attack, chuyển sang trạng thái Idle hoặc các trạng thái khác
        //        m_StateMachine.SetState(CharacterStateID.Idle);
        //    }
        //    return;
        //}
        m_Animator.SetFloat("x", m_UserInput.x);
        m_Animator.SetFloat("y", m_UserInput.y);
        if (!m_CharacterController.isGrounded || m_Input.jump)
        {
            m_StateMachine.SetState(CharacterStateID.Jump);
            Jump();
            return;
        }
        if (m_UserInput != Vector2.zero)
        {
            if (m_Input.sprint)
            {
                m_StateMachine.SetState(CharacterStateID.Run);
            }
            else
            {
                m_StateMachine.SetState(CharacterStateID.Walk);
            }
            return;
        }
        m_StateMachine.SetState(CharacterStateID.Idle);
    }
    private void SetDeath()
    {
        m_IsDead = true;
        ListenerManager.Instance.BroadCast(ListenType.ON_PLAYER_DEATH, this);

    }
    public bool IsPlayerDeath()
    {
        return m_IsDead;
    }
    public float GetPlayerCurrentHeath()
    {
        return m_Heath;
    }
    public float GetPlayerCurrentMana()
    {
        return m_Mana;
    }
    public float GetPlayerCurrentStamina()
    {
        return m_Stanima;
    }
    public float GetPlayerMaxHeath()
    {
        return m_MaxHeath;
    }
    public float GetPlayerMaxMana()
    {
        return m_MaxMana;
    }
    public float GetPlayerMaxStamina()
    {
        return m_MaxStanima;
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
    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = m_RootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        m_CharacterController.Move(stepForwardAmount + stepDownAmount);
        m_RootMotion = Vector3.zero;
        
        if (!m_CharacterController.isGrounded)
        {
            SetInAir(0);
        }
    }
    private void UpdateInAir()
    {
        m_Velocity.y -= m_Gravity * Time.fixedDeltaTime;
        Vector3 airDisplacement = m_Velocity * Time.fixedDeltaTime;
        airDisplacement += CalculateAircontrol();
        m_CharacterController.Move(airDisplacement);
        isJumping = !m_CharacterController.isGrounded;
        //m_RootMotion = Vector3.zero;
        m_Animator.SetBool("Grounded", m_CharacterController.isGrounded);
        m_Input.jump = isJumping;

    }

    private void OnAnimatorMove()
    {
        m_RootMotion += m_Animator.deltaPosition;
    }

    private void Jump()
    {
        if (!isJumping)
        {
          float jumpVelocity = Mathf.Sqrt(2 * m_Gravity * m_JumpHeight);
          SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        m_Velocity = m_Animator.velocity * jumpDamp * groundSpeed;
        m_Velocity.y = jumpVelocity;
    }

    private Vector3 CalculateAircontrol()
    {
        return ((transform.forward * m_Input.move.y) + (transform.right * m_Input.move.x)) * (airControl / 100);
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
    public void OnFootStep()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_FOOTSTEP);
        }
    }
    public void OnAttack1()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_MELEEATTACK1);
        }
    }
    public void OnIdle()
    {

    }
    //public void OnJump()
    //{
    //    if (AudioManager.HasInstance)
    //    {
    //        AudioManager.Instance.PlaySE(AUDIO.SE_JUMP);
    //    }
    //}
    public float GetDamage()
    {
        return m_MeleeDamage + m_RangedDamage;
    } 
}