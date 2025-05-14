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
    private ScreenGame m_ScreenGame;
    public Vector3 m_RootMotion;
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
    private float m_RecoverTimer = 0f;
    private float m_RecoverInterval = 0.5f;

    public float m_JumpHeight;
    public float m_Gravity;
    public float m_StepDown;
    public float m_AirControl;
    public float jumpDamp;
    public float m_GroundSpeed;
    public float pushPower;
    public bool m_IsJumping;
    private bool m_IsDead;
    public StarterAssetsInputs m_Input;
    private PlayerStateMachine m_StateMachine;
    private int m_DefaultMouseSpeed = 200;

    public bool m_IsAttack;
    public bool m_IsHeavyAttack;
    public bool m_IsUseSkillA;
    public float m_SkillACost;
    public bool m_IsUseSkillB;
    public float m_SkillBCost;
    public bool m_IsUseSkillC;
    public float m_SkillCCost;
    public bool m_IsUseSkillD;
    public float m_SkillDCost;
    public bool m_IsUseSkillI;
    public float m_SkillICost;
    public float m_JumpCost = 10f;
    public float m_HeavyAttackCost = 10f;
    public float m_RunCost = 1f;

    private bool m_IsUnblockSkill1;
    private bool m_IsUnblockSkill2;
    private bool m_IsUnblockSkill3;
    private bool m_IsUnblockSkill4;
    private bool m_IsUnblockSkill5;
    public bool m_CanUseSkill1;
    public bool m_CanUseSkill2;
    public bool m_CanUseSkill3;
    public bool m_CanUseSkill4;
    public bool m_CanUseSkill5;

    private float m_Skill1Damage = 1.5f;
    private float m_Skill2Damage = 2f;
    private float m_Skill3Damage = 2.5f;
    private float m_Skill4Damage = 3f;
    private float m_Skill5Damage = 4f;

    private float m_Skill1Timer;
    private float m_Skill2Timer;
    private float m_Skill3Timer;
    private float m_Skill4Timer;
    private float m_Skill5Timer;
    private float m_Skill1IColdown = 5f;
    private float m_Skill2IColdown = 5f;
    private float m_Skill3IColdown = 10f;
    private float m_Skill4IColdown = 10f;
    private float m_Skill5IColdown = 15f;

    public Collider m_Collider;
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_StateMachine = GetComponent<PlayerStateMachine>();
        m_Input = GetComponent<StarterAssetsInputs>();
        m_Weapon = GetComponentInChildren<PlayerWeapon>();
        m_CharacterAiming = GetComponent<CharacterAiming>();
        m_Collider = GetComponent<Collider>();
        
    }
    private void OnEnable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UN_BLOCK_SKILL, OnUnBlockSkill);
        }
    }
    private void OnDisable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UN_BLOCK_SKILL, OnUnBlockSkill);
        }
    }
    void Start()
    {
        SetupDefault();
        m_ScreenGame = FindAnyObjectByType<ScreenGame>();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.SetPlayer(this);
        }
        m_CharacterAiming.xAxis.m_MaxSpeed = m_DefaultMouseSpeed;
        m_CharacterAiming.yAxis.m_MaxSpeed = m_DefaultMouseSpeed;
    }
    void Update()
    {
        if (!GameManager.HasInstance)
        {
            return;
        }
        if (IsPlayerDeath())
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }
        if (GameManager.Instance.GetGameState() != GameStateID.Start)
        {
            return;
        }
        m_UserInput.x = Input.GetAxis("Horizontal");
        m_UserInput.y = Input.GetAxis("Vertical");
        UpdateState();
        UpdatePopup();
        RecoverStat();
        OnSkillColdown();
    }
    private void OnSkillColdown()
    {
        if(m_IsUnblockSkill1)
        {
            m_Skill1Timer += Time.deltaTime;
            if (m_Skill1Timer >= m_Skill1IColdown)
            {
                m_Skill1Timer = 0f;
                m_CanUseSkill1 = true;
                ListenerManager.Instance.BroadCast(ListenType.UN_BLOCK_SKILL, 1);
            }
        }
        if (m_IsUnblockSkill2)
        {
            m_Skill2Timer += Time.deltaTime;
            if (m_Skill2Timer >= m_Skill2IColdown)
            {
                m_Skill2Timer = 0f;
                m_CanUseSkill2 = true;
                ListenerManager.Instance.BroadCast(ListenType.UN_BLOCK_SKILL, 2);
            }
        }
        if (m_IsUnblockSkill3)
        {
            m_Skill3Timer += Time.deltaTime;
            if (m_Skill3Timer >= m_Skill3IColdown)
            {
                m_Skill3Timer = 0f;
                m_CanUseSkill3 = true;
                ListenerManager.Instance.BroadCast(ListenType.UN_BLOCK_SKILL, 3);
            }
        }
        if (m_IsUnblockSkill4)
        {
            m_Skill4Timer += Time.deltaTime;
            if (m_Skill4Timer >= m_Skill4IColdown)
            {
                m_Skill4Timer = 0f;
                m_CanUseSkill4 = true;
                ListenerManager.Instance.BroadCast(ListenType.UN_BLOCK_SKILL, 4);
            }
        }
        if (m_IsUnblockSkill5)
        {
            m_Skill5Timer += Time.deltaTime;
            if (m_Skill5Timer >= m_Skill5IColdown)
            {
                m_Skill5Timer = 0f;
                m_CanUseSkill5 = true;
                ListenerManager.Instance.BroadCast(ListenType.UN_BLOCK_SKILL, 5);
            }
        }

    }
    private void OnUnBlockSkill(object value)
    {
        if (value == null)
        {
            return;
        }
        if (value is int nvalue)
        {
            if (nvalue == 1 && !m_IsUnblockSkill1)
            {
                m_IsUnblockSkill1 = true;
                m_CanUseSkill1 = true;
            }
            else if (nvalue == 2 && !m_IsUnblockSkill2)
            {
                m_IsUnblockSkill2 = true;
                m_CanUseSkill2 = true;
            }
            else if (nvalue == 3 && !m_IsUnblockSkill3)
            {
                m_IsUnblockSkill3 = true;
                m_CanUseSkill3 = true;
            }
            else if (nvalue == 4 && !m_IsUnblockSkill4)
            {
                m_IsUnblockSkill4 = true;
                m_CanUseSkill4 = true;
            }
            else if (nvalue == 5 && !m_IsUnblockSkill5)
            {
                m_IsUnblockSkill5 = true;
                m_CanUseSkill5 = true;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            m_Heath -= damage;
            if (m_Heath <= 0)
            {
                SetDeath();
            }
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, this);
        }
    }
    private void UpdatePopup()
    {
        if (UIManager.Instance == null)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.F12) && !UIManager.Instance.GetExistPopup<PopupMission>())
        {
            UIManager.Instance.ShowPopup<PopupMission>();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F3) && !UIManager.Instance.GetExistPopup<PopupPlayerInfomation>())
        {
            UIManager.Instance.ShowPopup<PopupPlayerInfomation>();
            //SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F1) && !UIManager.Instance.GetExistPopup<PopupTutorials>())
        {
            UIManager.Instance.ShowPopup<PopupTutorials>();
            //SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !UIManager.Instance.GetExistPopup<PopupPauseGame>())
        {
            UIManager.Instance.ShowPopup<PopupPauseGame>();
            //SetMouseSpeed(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Insert) && !UIManager.Instance.GetExistPopup<PopupCheatGame>())
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
            m_Input.cursorInputForLook = false;
        }
    }
    private void RecoverStat()
    {
        if (m_StateMachine != null)
        {
            if(m_StateMachine.m_CurrentStateID == CharacterStateID.Walk || m_StateMachine.m_CurrentStateID == CharacterStateID.Idle)
            {
                m_RecoverTimer += Time.deltaTime;
                if (m_RecoverTimer >= m_RecoverInterval)
                {
                    RecoverHeath(5);
                    RecoverMana(1);
                    RecoverStamina(1);
                    m_RecoverTimer = 0f;
                }
            }
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
            m_StepDown = DataManager.Instance.GlobalConfig.stepDown;
            m_AirControl = DataManager.Instance.GlobalConfig.airControl;
            jumpDamp = DataManager.Instance.GlobalConfig.jumpDamp;
            m_GroundSpeed = DataManager.Instance.GlobalConfig.groundSpeed;
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

            m_SkillACost = 5f;
            m_SkillBCost = 10f;
            m_SkillCCost = 15f;
            m_SkillDCost = 20f;
            m_SkillICost = 25f;

            ListenerManager.Instance.BroadCast(ListenType.UPDATE_USER_INFO, this);
        }
        if(m_Collider != null)
        {
            m_Collider.enabled = true;
        }

    }
    private void UpdateState()
    {
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.Death)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.Attack)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.HeavyAttack)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.Jump)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.SkillA)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.SkillB)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.SkillC)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.SkillD)
        {
            return;
        }
        if (m_StateMachine.m_CurrentStateID == CharacterStateID.SkillI)
        {
            return;
        }
        m_Animator.SetFloat("x", m_UserInput.x);
        m_Animator.SetFloat("y", m_UserInput.y);

      //Debug.Log("IIIIIIIIIIIIIIIIIIIIIII");
      // Kiểm tra trạng thái tấn công
        if (Input.GetKeyDown(KeyCode.Mouse0) && m_CharacterController.isGrounded)
      {
          if (!m_IsAttack)
          {
              m_IsAttack = true;
              m_StateMachine.SetState(CharacterStateID.Attack);

          }
          return;
      }
        if (Input.GetKeyDown(KeyCode.Mouse1) && m_CharacterController.isGrounded && m_Stanima >= m_HeavyAttackCost)
      {
          if (!m_IsHeavyAttack)
          {
              m_IsHeavyAttack = true;
              m_StateMachine.SetState(CharacterStateID.HeavyAttack);
          }

          return;
      }
        // Skill
        if (Input.GetKeyUp(KeyCode.Alpha1) && m_Mana >= m_SkillACost && m_CharacterController.isGrounded && !m_IsUseSkillA && m_IsUnblockSkill1 && m_CanUseSkill1)
        {
            m_IsUseSkillA = true;
            m_CanUseSkill1 = false;
            m_StateMachine.SetState(CharacterStateID.SkillA);
            
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.BLOCK_SKILL, 1);
                m_Skill1Timer = 0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) && m_Mana >= m_SkillBCost && m_CharacterController.isGrounded && !m_IsUseSkillB && m_IsUnblockSkill2 && m_CanUseSkill2)
        {
            m_IsUseSkillB = true;
            m_CanUseSkill2 = false;
            m_StateMachine.SetState(CharacterStateID.SkillB);
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.BLOCK_SKILL, 2);
                m_Skill2Timer = 0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) && m_Mana >= m_SkillCCost && m_CharacterController.isGrounded && !m_IsUseSkillC && m_IsUnblockSkill3 && m_CanUseSkill3)
        {
            m_IsUseSkillC = true;
            m_CanUseSkill3 = false;
            m_StateMachine.SetState(CharacterStateID.SkillC);
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.BLOCK_SKILL, 3);
                m_Skill3Timer = 0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) && m_Mana >= m_SkillDCost && m_CharacterController.isGrounded && !m_IsUseSkillD && m_IsUnblockSkill4 && m_CanUseSkill4)
        {
            m_IsUseSkillD = true;
            m_CanUseSkill4 = false;
            m_StateMachine.SetState(CharacterStateID.SkillD);
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.BLOCK_SKILL, 4);
                m_Skill4Timer = 0f;
            }
            return;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) && m_Mana >= m_SkillICost && m_CharacterController.isGrounded && !m_IsUseSkillI && m_IsUnblockSkill5 && m_CanUseSkill5)
        {
            m_IsUseSkillI = true;
            m_CanUseSkill5 = false;
            m_StateMachine.SetState(CharacterStateID.SkillI);
            if (ListenerManager.HasInstance)
            {
                ListenerManager.Instance.BroadCast(ListenType.BLOCK_SKILL, 5);
                m_Skill5Timer = 0f;
            }
            return;
        }
        //===============================
        if (Input.GetKeyDown(KeyCode.Space) && m_Stanima >= m_JumpCost && m_CharacterController.isGrounded)
        {
            if (!m_IsJumping)
            {
                m_Input.jump = false;
                m_StateMachine.SetState(CharacterStateID.Jump);
            }
            return;
        }
        if (m_UserInput != Vector2.zero)
        {
            if (m_Input.sprint && m_Stanima >= m_RunCost)
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
        m_Heath = 0;
        m_IsDead = true;
        SetMouseSpeed(0);
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
    public void RecoverHeath(float value)
    {
        if(m_Heath >= m_MaxHeath)
        {
            return;
        }
        if (m_Heath + value > m_MaxHeath)
        {
            value = m_MaxHeath - m_Heath;
        }
        m_Heath += value;
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, this);
        if (m_Heath >= m_MaxHeath) { return; }
    }
    public float GetCurrentMana()
    {
        return m_Mana;
    }
    public void RecoverMana(float value)
    {
        if (m_Mana >= m_MaxMana)
        {
            return;
        }
        if (m_Mana + value > m_MaxMana)
        {
            value = m_MaxMana - m_Mana;
        }
        m_Mana += value;
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_MANA, this);
        if (m_Mana >= m_MaxMana) 
        { 
            return; 
        }
        
    }
    public void RemainMana(float value)
    {
        m_Mana -= value;
        if (m_Mana < 0) { m_Mana = 0; }
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_MANA, this);
    }
    public float GetCurrentStamina()
    {
        return m_Stanima;
    }
    public void RecoverStamina(float value)
    {
        if (m_Stanima >= m_MaxStanima)
        {
            return;
        }
        if (m_Stanima + value > m_MaxStanima)
        {
            value = m_MaxStanima - m_Stanima;
        }
        m_Stanima += value;
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_STAMINA, this);
        if (m_Stanima >= m_MaxStanima)
        { return; }

    }
    public void RemainStamina(float value)
    {
        m_Stanima -= value;
        if (m_Stanima < 0) { m_Stanima = 0; }
        ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_STAMINA, this);
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
        OnRunAndWalk();
    }
    public void OnRunAndWalk()
    {
        if (!m_CharacterController.isGrounded)
        {
            m_Velocity.y -= m_Gravity * Time.fixedDeltaTime;
            Vector3 airDisplacement = m_Velocity * Time.fixedDeltaTime;
            airDisplacement += CalculateAircontrol();
            m_CharacterController.Move(airDisplacement);
            m_RootMotion = Vector3.zero;
        }
        else
        {
            Vector3 stepForwardAmount = m_RootMotion * m_GroundSpeed;
            Vector3 stepDownAmount = Vector3.down * m_StepDown;
            m_CharacterController.Move(stepForwardAmount + stepDownAmount);
            m_RootMotion = Vector3.zero;
        }
    }
    public void Jump()
    {
        m_Velocity.y -= m_Gravity * Time.deltaTime;
        Vector3 airDisplacement = m_Velocity * Time.deltaTime;
        airDisplacement += CalculateAircontrol();
        m_CharacterController.Move(airDisplacement);
        SetInAir(m_JumpHeight);
    }
    public void Jumping()
    {
        m_Velocity.y -= m_Gravity * Time.deltaTime;
        //Debug.Log(m_Velocity.y);
        Vector3 airDisplacement = m_Velocity * Time.deltaTime;
        airDisplacement += CalculateAircontrol();
        m_CharacterController.Move(airDisplacement);
        m_RootMotion = Vector3.zero;
    }
    private void SetInAir(float jumpVelocity)
    {
        m_Velocity = m_Animator.velocity * jumpDamp * m_GroundSpeed;
        m_Velocity.y = jumpVelocity;
    }
    private Vector3 CalculateAircontrol()
    {
        return ((transform.forward * m_Input.move.y) + (transform.right * m_Input.move.x)) * (m_AirControl / 100);
    }
    private void OnAnimatorMove()
    {
        m_RootMotion += m_Animator.deltaPosition;
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Rigidbody body = hit.collider.attachedRigidbody;
    //    if (body == null || body.isKinematic)
    //        return;
    //    if (hit.moveDirection.y < -0.3f)
    //        return;
    //    Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
    //    body.linearVelocity = pushDir * pushPower;
    //} 
    public void OnFootStep()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_FOOTSTEP);
        }
    }
    public void JumpSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_FOOTSTEP);
        }
    }
    public void AttackSound()
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
    public float GetSkillDamage(int skill)
    {
        if (skill == 1)
        {
            return m_Skill1Damage;
        }
        if (skill == 2)
        {
            return m_Skill2Damage;
        }
        if (skill == 3)
        {
            return m_Skill3Damage;
        }
        if (skill == 4)
        {
            return m_Skill4Damage;
        }
        if (skill == 5)
        {
            return m_Skill5Damage;
        }
        return 1f;
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