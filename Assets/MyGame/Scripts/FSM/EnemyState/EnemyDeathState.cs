using UnityEngine;
public class EnemyDeathState<T> : State<Enemy>
{
    public EnemyDeathState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", true);
        }
    }

    public override void Update()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Agent.SetDestination(Vector3.zero);
            m_Character.m_Agent.isStopped = true;
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", false);
        }
    }
}
