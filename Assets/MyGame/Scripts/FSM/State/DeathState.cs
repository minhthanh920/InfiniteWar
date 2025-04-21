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
                player.m_Animator.SetBool("IsDeath", true);
            }
        }
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Animator.SetBool("IsDeath", true);
            }
        }
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("IsDeath", false);
            }
        }
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Animator.SetBool("IsDeath", false);
            }
        }
    }
}
