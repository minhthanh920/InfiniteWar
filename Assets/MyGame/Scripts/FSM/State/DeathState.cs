using UnityEngine;
public class DeathState<T> : State<T>
{
    public DeathState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
    }

    public override void Enter()
    {
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("IsDead", true);
            }
        }
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {

                enemy.m_Animator.SetBool("IsDead", true);
            }
        }
    }

    public override void Update()
    {
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.SetDestination(Vector3.zero);
                enemy.m_Agent.isStopped = true;
            }
        }
        
    }

    public override void Exit()
    {
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("IsDead", false);
            }
        }
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Animator.SetBool("IsDead", false);
            }
        }
    }
}
