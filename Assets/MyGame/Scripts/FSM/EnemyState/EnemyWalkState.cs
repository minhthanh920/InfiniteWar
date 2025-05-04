public class EnemyWalkState : State<Enemy>
{
    public EnemyWalkState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Walk", true);
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Walk", true);
        }
    }
}
