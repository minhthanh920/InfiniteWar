using UnityEngine;

public class JumpState<T> : State<T>
{
    private float m_Progress;
    public JumpState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
    }

    public override void Enter()
    {

        if (character is Enemy enemy)
        {
            enemy.m_Agent.isStopped = true;
            enemy.m_Agent.SetDestination(Vector3.zero);
            enemy.m_Animator.SetBool("IsJumping", true);
        }
        if (character is Player player)
        {
            player.m_Animator.SetBool("IsJumping", true);
        }
    }

    public override void Update()
    {
        if (character is Player player)
        {
            AnimatorStateInfo stateInfo = player.m_Animator.GetCurrentAnimatorStateInfo(0);
            m_Progress = stateInfo.normalizedTime % 1f; // normalizedTime có thể > 1 khi loop
            if (m_Progress >= 1f)
            {
                player.m_Input.jump = false;
                player.m_Animator.SetBool("FreeFall", true);
            }

        }
    }

    public override void Exit()
    {
        //Debug.Log($"{typeof(T).Name} rời khỏi trạng thái IDLE.");
        if (character is Enemy enemy)
        {
            enemy.m_Animator.SetBool("IsJumping", false);
        }
        if (character is Player player)
        {
            player.m_Animator.SetBool("IsJumping", false);
            player.m_Animator.SetBool("FreeFall", false);
        }
    }
}

