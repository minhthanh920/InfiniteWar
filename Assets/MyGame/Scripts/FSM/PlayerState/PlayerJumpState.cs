using UnityEngine;
using UnityEngine.Windows;

public class PlayerJumpState : State<Player>
{
    public PlayerJumpState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }

    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            Debug.Log("vao state jum");
            m_Character.m_Animator.SetBool("IsJumping", true);
            
            // Tính vận tốc nhảy
            //float jumpVelocity = Mathf.Sqrt(2 * m_Character.m_Gravity * m_Character.m_JumpHeight);

            // Thiết lập vận tốc ban đầu cho nhảy
            //m_Character.m_Velocity = m_Character.m_Animator.velocity * m_Character.m_JumpDamp * m_Character.m_GroundSpeed;
            //m_Character.m_Velocity.y = jumpVelocity;
        }
        //m_Character.OnJump();
    }

    public override void Update()
    {
        // Áp lực hấp dẫn
        //m_Character.m_Velocity.y -= m_Character.m_Gravity * Time.fixedDeltaTime;
        //
        //// Điều khiển trên không
        //Vector3 airDisplacement = m_Character.m_Velocity * Time.fixedDeltaTime;
        //airDisplacement += ((m_Character.transform.forward * m_Character.m_Input.move.y) +
        //                    (m_Character.transform.right * m_Character.m_Input.move.x)) *
        //                    (m_Character.m_AirControl / 100f); m_Character.m_CharacterController.Move(airDisplacement);
        //m_Character.m_CharacterController.Move(airDisplacement);
        //// Nếu nhân vật chạm đất
        //if (m_Character.m_CharacterController.isGrounded)
        //{
        //    m_Character.m_Input.jump = false;
        //    m_Character.m_Velocity.y = 0;
        //    m_StateMachine.SetState(CharacterStateID.Idle);
        //}
    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsJumping", false);
            
        }
        m_Character.OnJump();
    }
}

