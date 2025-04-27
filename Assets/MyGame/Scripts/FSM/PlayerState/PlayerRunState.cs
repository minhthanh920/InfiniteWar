public class PlayerRunState : State<Player>
{
    public PlayerRunState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Run", true);
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Run", false);
        }
    }
}

