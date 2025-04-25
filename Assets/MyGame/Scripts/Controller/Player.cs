using DG.Tweening.Core.Easing;
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
    [SerializeField]
    private PlayerSO m_PlayerSO;
    public CharacterController m_CharacterController;

    private string m_GameOver = "GameOver";
    private Vector3 m_RootMotion;
    private Vector3 m_Velocity;
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
    private bool m_IsDead;
    public StarterAssetsInputs m_Input;
    private PlayerStateMachine m_StateMachine;
    private void OnEnable()
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
    }
    void Update()
    {   
        if(IsPlayerDeath())
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }
        m_UserInput.x = Input.GetAxis("Horizontal");
        m_UserInput.y = Input.GetAxis("Vertical");
        UpdateState();
        //UpdateIsSprinting();

    }
    public void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            m_Heath -= damage;
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, m_Heath / m_PlayerSO.m_Heath);
            if (m_Heath <= 0)
            {
                SetDeath();
            }
        }
    }
    public void SetupDefault()
    {
        m_StateMachine.AddState(CharacterStateID.Idle, new IdleState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Walk, new WalkState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Run, new RunState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Attack, new AttackState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Death, new DeathState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Jump, new JumpState<Player>(m_StateMachine, this));
        m_StateMachine.SetState(CharacterStateID.Idle);
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
        if (m_PlayerSO != null)
        {
            m_Heath = m_PlayerSO.m_Heath;
            m_Speed = m_PlayerSO.m_Speed;
            m_Stanima = m_PlayerSO.m_Stamina;
            m_MeleeDamage = m_PlayerSO.m_MeleeDamage;
            m_RangedDamage = m_PlayerSO.m_RangeDamage;
            m_IsDead = false;
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, m_Heath);
        }
        else
        {
            Debug.Log("PlayerSO is null");
        }
    }
    private void UpdateState()
    {
        Debug.Log($"m_Input.attack : {m_Input.attack}");
        if (m_Heath <= 0f)
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }

        if (m_Input.attack)
        {
            m_StateMachine.SetState(CharacterStateID.Attack);
            return;
        }
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
        ListenerManager.Instance.BroadCast(ListenType.ON_PLAYER_DEATH, m_GameOver);

    }
    private bool IsPlayerDeath()
    {
        return m_IsDead;
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
        m_Velocity.y -= gravity * Time.fixedDeltaTime;
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
          float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
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
    public float GetDamage()
    {
        return m_MeleeDamage + m_RangedDamage;
    } 
}