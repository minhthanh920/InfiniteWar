using UnityEngine;

public class EnemyIdleState<T> : State<Enemy>
{
    public EnemyIdleState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Idle", true);
        }
    }

    public override void Update()
    {
        if (Vector3.Distance(m_Character.transform.position, m_Character.GetPlayerPos()) <= 1.5f)
        {
            m_StateMachine.SetState(CharacterStateID.Attack);
        }
        else
        {
            if (m_Character.m_AttackColdown > 0)
            {
                return;
            }
            m_StateMachine.SetState(CharacterStateID.Chasing);
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Idle", false);
        }
    }
}

