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
    public CharacterAiming m_CharacterAiming;

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
    private int m_DefaultMouseSpeed = 200;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<PlayerStateMachine>();
        m_Input = GetComponent<StarterAssetsInputs>();
        m_Weapon = GetComponentInChildren<PlayerWeapon>();
        m_CharacterAiming = GetComponent<CharacterAiming>();

    }
    void Start()
    {
        SetupDefault();
        if(GameManager.HasInstance)
        {
            GameManager.Instance.SetPlayer(this);
        }
        m_CharacterAiming.xAxis.m_MaxSpeed = m_DefaultMouseSpeed;
        m_CharacterAiming.yAxis.m_MaxSpeed = m_DefaultMouseSpeed;
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
        UpdatePopup();
        UpdateUseSkill();
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
    private void UpdateUseSkill()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_USE_SKILL, 1);
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_USE_SKILL, 2);
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_USE_SKILL, 3);
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_USE_SKILL, 4);
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.UPDATE_USE_SKILL, 5);
            }
            return;
        }
    }    
    private void UpdatePopup()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestoreMouseSpeed();
            return;
        }
        if (UIManager.Instance == null)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.F12))
        {
            UIManager.Instance.ShowPopup<PopupMission>();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UIManager.Instance.ShowPopup<PopupPlayerInfomation>();
            SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            UIManager.Instance.ShowPopup<PopupTutorials>();
            SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UIManager.Instance.ShowPopup<PopupPauseGame>();
            SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UIManager.Instance.ShowPopup<PopupPauseGame>();
            SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            UIManager.Instance.ShowPopup<PopupCheatGame>();
            return;
        }
    }
    public void SetMouseSpeed(int value)
    {
        if (m_CharacterAiming != null)
        {
            m_CharacterAiming.xAxis.m_MaxSpeed = value;
            m_CharacterAiming.yAxis.m_MaxSpeed = value;
            Cursor.visible = true; // Hiển thị con trỏ chuột
            Cursor.lockState = CursorLockMode.None; // Cho phép con trỏ di chuyển tự do
        }
    }
    public void RestoreMouseSpeed()
    {
        if (m_CharacterAiming != null)
        {
            m_CharacterAiming.xAxis.m_MaxSpeed = m_DefaultMouseSpeed;
            m_CharacterAiming.yAxis.m_MaxSpeed = m_DefaultMouseSpeed;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
    public float GetCurrentHeath()
    {
        return m_Heath;
    }
    public float GetCurrentMana()
    {
        return m_Mana;
    }
    public float GetCurrentStamina()
    {
        return m_Stanima;
    }
    public float GetMaxHeath()
    {
        return m_MaxHeath;
    }
    public float GetMaxMana()
    {
        return m_MaxMana;
    }
    public float GetMaxStamina()
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
    public void OnJump()
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
    public void SetDamage(int damage)
    {
        m_MeleeDamage = damage;
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_DAMAGE, this);
    }
    public void AddDamage(int damage)
    {
        m_MeleeDamage += damage;
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_DAMAGE, this);
    }
    public void RestoreFull()
    {
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
    }    
}