using System.Collections;
using UnityEngine;
public class EnemyDeathState : State<Enemy>
{
    private float m_RespawnInterval = 5f;
    private float m_RespawnTimer = 0f;
    public EnemyDeathState(BaseStateMachine<Enemy> stateMachine, Enemy enemy) : base(stateMachine, enemy) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", true);
        }
        m_Character.m_WeaponCollider.enabled = false;
        m_Character.m_EnemyCollider.enabled = false;
        m_Character.m_Agent.isStopped = true;
        m_Character.m_Agent.ResetPath();
    }

    public override void Update()
    {
        if (m_Character.m_Animator != null)
        {
            m_RespawnTimer += Time.deltaTime;
            if (m_RespawnTimer >= m_RespawnInterval)
            {
                m_Character.Respawn();
                m_RespawnTimer = 0f;
            }    
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", false);
        }
        m_Character.m_WeaponCollider.enabled = true;
        m_Character.m_EnemyCollider.enabled = true;
        m_Character.m_Agent.isStopped = false;
        m_Character.m_Agent.ResetPath(); // Xóa đường dẫn hiện tại
        m_Character.m_Agent.SetDestination(m_Character.GetPlayerPos());
    }
}
