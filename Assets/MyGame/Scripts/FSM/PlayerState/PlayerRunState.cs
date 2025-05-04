using UnityEngine;

public class PlayerRunState : State<Player>
{
    public PlayerRunState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    private float m_FootstepTimer = 0f;
    [SerializeField] private float m_FootstepInterval = 0.45f;
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Run", true);
        }
    }

    public override void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_Character.m_Input.sprint = false;
        }
        if (m_Character.m_CharacterController.isGrounded)
        {
            m_FootstepTimer += Time.deltaTime;

            if (m_FootstepTimer >= m_FootstepInterval)
            {
                m_Character.OnFootStep();
                m_FootstepTimer = 0f;
            }
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Run", false);
        }
    }
}

