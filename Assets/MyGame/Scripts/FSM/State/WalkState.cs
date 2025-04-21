using UnityEngine;
public class WalkState<T> : State<T>
{
    public WalkState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
    }

    public override void Enter()
    {
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("Walk", true);
            }
        }
    }

    public override void Update()
    {
        //if (character is Player player)
        //{
        //    if (player.m_Animator != null)
        //    {
        //        if (player.m_UserInput == Vector2.zero)
        //        {
        //            Exit();
        //        }
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
                player.m_Animator.SetBool("Walk", false);
            }
        }
    }
}
