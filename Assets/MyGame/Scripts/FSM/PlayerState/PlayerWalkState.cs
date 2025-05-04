using UnityEngine;

public class PlayerWalkState : State<Player>
{
    public PlayerWalkState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    private float m_FootstepTimer = 0f;
    [SerializeField] private float m_FootstepInterval = 0.6f;
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Walk", true);
        }
    }

    public override void Update()
    {
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
            m_Character.m_Animator.SetBool("Walk", false);
        }
    }
}

