using UnityEditor;
using UnityEngine;

public class EnemyAttackState: State<Enemy>
{
    public EnemyAttackState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }
    public override void Enter()
    {
        //Debug.Log($"{typeof(T).Name} vào trạng thái Attack.");
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Agent.isStopped = true;
            m_Character.m_Agent.SetDestination(Vector3.zero);
            m_Character.m_Animator.SetBool("Attack", true);
            m_Character.m_AttackColdown = m_Character.m_EnemyS0.m_AttackTime;
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
        if (Vector3.Distance(m_Character.transform.position, m_Character.GetPlayerPos()) > 1.5f)
        {
            if (m_Character.m_AttackColdown > 0)
            {
                return;
            }
            m_StateMachine.SetState(CharacterStateID.Chasing);
        }
        else
        {
            m_Character.m_AttackColdown = m_Character.m_EnemyS0.m_AttackTime;
        }
        
    }

    public override void Exit()
    {
       // Debug.Log($"{typeof(T).Name} rời khỏi trạng thái Attack.");
       if (m_Character.m_Animator != null)
       {
           m_Character.m_Agent.isStopped = false;
           m_Character.m_Animator.SetBool("Attack", false);
       }
    }
}
