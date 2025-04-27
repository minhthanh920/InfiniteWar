public class PlayerIdleState : State<Player>
{
    public PlayerIdleState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Idle", true);
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Idle", false);
        }
    }
}

