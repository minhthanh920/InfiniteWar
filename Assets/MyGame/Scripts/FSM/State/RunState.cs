using UnityEngine;
public class RunState<T> : State<T>
{
    public RunState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
    }

    public override void Enter()
    {
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("Run", true);
            }
        }
    }

    public override void Update()
    {
       //if (character is Player player)
       //{
       //    if (player.m_Animator != null)
       //    {
       //        player.m_UserInput.x = Input.GetAxis("Horizontal");
       //        player.m_UserInput.y = Input.GetAxis("Vertical");
       //        player.m_Animator.SetFloat("x", player.m_UserInput.x);
       //        player.m_Animator.SetFloat("y", player.m_UserInput.y);
       //    }
       //}

    }

    public override void Exit()
    {
        //Debug.Log($"{typeof(T).Name} ngừng đuổi theo.");
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("Run", false);
            }
        }
    }
}
