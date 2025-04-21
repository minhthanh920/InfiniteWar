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
    public int m_ClickAttackCount;
    public PlayerWeapon m_Weapon;
    [SerializeField]
    private PlayerSO m_PlayerSO;
    private CharacterController m_CharacterController;

    private string m_GameOver = "GameOver";
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
    private bool m_IsDead;
    public StarterAssetsInputs m_Input;
    private PlayerStateMachine m_StateMachine;

    public float m_AttackTime;
    public float m_DecayHitTime;

    
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
        //if (m_Weapon != null)
        //{
        //    Debug.Log("OK");
        //}
        SetupDefault();

        m_StateMachine.AddState(CharacterStateID.Idle, new IdleState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Walk, new WalkState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Run, new RunState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Attack, new AttackState<Player>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Death, new DeathState<Player>(m_StateMachine, this));
        m_StateMachine.SetState(CharacterStateID.Idle);
    }

    void Update()
    {   
        if(IsPlayerDeath())
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            if (GameManager.HasInstance)
            {
                GameManager.Instance.SetGameState(GameStateID.GameOver);
            }
            return;
        }
        Init();



        //UpdateIsSprinting();

    }
    private void Init()
    {
        m_Animator.SetFloat("x", m_Input.move.x);
        m_Animator.SetFloat("y", m_Input.move.y);
        // m_StateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (m_Input.attack)
        {
            m_StateMachine.SetState(CharacterStateID.Attack);
        }
        else
        {
            if (m_Input.jump)
            {
                Jump();
            }
            if (m_Input.move != Vector2.zero)
            {
                if (m_Input.sprint)
                {
                    m_StateMachine.SetState(CharacterStateID.Run);
                }
                else
                {
                    m_StateMachine.SetState(CharacterStateID.Walk);
                }
            }
            else
            {
                m_StateMachine.SetState(CharacterStateID.Idle);
            }
        }
    }
    public void SetDecayHitTime()
    {
        m_DecayHitTime = 1f;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log($"Player Nhan Damage : {damage}");
        if (damage > 0)
        {
            m_Heath -= damage;
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, m_Heath / m_PlayerSO.m_Heath);
            if (m_Heath <= 0)
            {
                //Debug.Log($"Player Nhan Damage : {damage}");
                //ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, m_Heath);
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
            m_IsDead = false;
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_PLAYER_HEALTH, m_Heath);
        }
        else
        {
            Debug.Log("PlayerSO is null");
        }
    }
    private void SetDeath()
    {
        m_IsDead = true;
        if (GameManager.HasInstance)
        {
            GameManager.Instance.SetGameState(GameStateID.GameOver);
        }
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
        m_CharacterController.Move(airDisplacement);
        isJumping = !m_CharacterController.isGrounded;
        rootMotion = Vector3.zero;
        m_Animator.SetBool("IsJumping", isJumping);
        m_Input.jump = isJumping;

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
            //m_Input.jump = false;
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = m_Animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
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