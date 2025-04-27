using UnityEngine.TextCore.Text;
using UnityEngine;

public class PlayerJumpState : State<Player>
{
    public PlayerJumpState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsJumping", true);
        }
    }

    public override void Update()
    {
        //AnimatorStateInfo stateInfo = m_Character.m_Animator.GetCurrentAnimatorStateInfo(0);
        //m_Progress = stateInfo.normalizedTime % 1f; // normalizedTime có thể > 1 khi loop
        //if (m_Progress >= 1f)
        //{
        //    m_Character.m_Input.jump = false;
        //    m_Character.m_Animator.SetBool("FreeFall", true);
        //}
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsJumping", false);
        }
    }
}

