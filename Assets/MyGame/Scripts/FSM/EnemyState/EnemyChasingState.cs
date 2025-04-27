using UnityEngine;
public class EnemyChasingState<T> : State<Enemy>
{
    public EnemyChasingState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Agent.isStopped = false;
            m_Character.m_Animator.SetBool("Run", true);
        }
    }

    public override void Update()
    {
        if (m_Character.IsDead())
        {
            m_Character.m_Agent.isStopped = true;
            m_Character.m_Agent.SetDestination(Vector3.zero);
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }
        if (m_Character.m_AttackColdown > 0f)
        {
            m_Character.m_Agent.isStopped = true;
            m_Character.m_Agent.SetDestination(Vector3.zero);
            return;
        }
        else if(Vector3.Distance(m_Character.transform.position, m_Character.GetPlayerPos()) <=2f)
        {
            m_StateMachine.SetState(CharacterStateID.Attack);
        }
        else
        {
           m_Character.m_Agent.SetDestination(m_Character.GetPlayerPos());
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Run", false);
            m_Character.m_Agent.isStopped = true;
            m_Character.m_Agent.SetDestination(Vector3.zero);

        }
    }
}
