using UnityEngine;
public class EnemyChasingState : State<Enemy>
{
    private Vector3 m_DirectionToPlayer;
    private float m_DistanceToPlayer;
    public EnemyChasingState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            //m_Character.m_Agent.isStopped = false;
            m_Character.m_Animator.SetBool("Run", true);
        }
        m_Character.m_Agent.isStopped = false;
        m_Character.m_Agent.SetDestination(m_Character.GetPlayerPos());
    }

    public override void Update()
    {
        if (m_Character.IsDead())
        {
            if(m_Character.m_Agent)
            {
                m_StateMachine.SetState(CharacterStateID.Death);
                return;
            }

        }
        m_DistanceToPlayer = Vector3.Distance(m_Character.transform.position, m_Character.GetPlayerPos());
        m_DirectionToPlayer = m_Character.GetPlayerPos() - m_Character.transform.position;
        m_DirectionToPlayer.Normalize();
        if (m_Character.m_AttackColdown > 0f)
        {
            m_Character.m_Agent.isStopped = true;
            m_Character.m_Agent.ResetPath();
            return;
        }
        else if (m_DistanceToPlayer <= 2f)
        {
            if (m_Character.m_AttackColdown > 0f)
            {
                return;   
            }
            float dotProduct = Vector3.Dot(m_Character.transform.forward, m_DirectionToPlayer);

            if (dotProduct > 0.7f)
            {
                // Enemy đang đối mặt với player, thực hiện tấn công
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
        }
        else
        {
            if (m_Character.m_Agent.isOnNavMesh)
            {
                m_Character.m_Agent.SetDestination(m_Character.GetPlayerPos());
            }
            else
            {
                Debug.LogWarning("Enemy không nằm trên NavMesh!");
                return; // hoặc có thể đợi vài frame rồi retry
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
