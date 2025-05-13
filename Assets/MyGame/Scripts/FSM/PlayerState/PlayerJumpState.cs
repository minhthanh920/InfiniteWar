using UnityEngine;
using UnityEngine.Windows;

public class PlayerJumpState : State<Player>
{
    private Vector3 m_Velocity; // Vận tốc của nhân vật trong trạng thái Jump

    public PlayerJumpState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            //Debug.Log("vao state jum");
            m_Character.m_Animator.SetBool(CONST.JUMP, true);
            m_Character.m_IsJumping = true;
            m_Character.JumpSound();
            m_Character.RemainStamina(m_Character.m_JumpCost);
        }
        m_Character.Jump();
    }

    public override void Update()
    {
        m_Character.Jumping();
        if (m_Character.m_CharacterController.isGrounded)
        {
            m_StateMachine.SetState(CharacterStateID.Idle);
        }
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool(CONST.JUMP, false);
            
        }
        m_Character.m_IsJumping = false;
        m_Character.JumpSound();
        m_Character.m_RootMotion = Vector3.zero;
    }
}

