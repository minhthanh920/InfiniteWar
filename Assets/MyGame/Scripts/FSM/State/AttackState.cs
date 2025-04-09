using UnityEngine;

public class AttackState<T> : State<T>
{
    //private Animator m_Animator;
    public AttackState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
        //if (character is MonoBehaviour mb)
        //{
        //    m_Animator = mb.GetComponent<Animator>();
        //}
    }
    public override void Enter()
    {
        Debug.Log($"{typeof(T).Name} vào trạng thái Attack.");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.isStopped = true;
                enemy.m_Agent.SetDestination(Vector3.zero);
                enemy.m_Animator.SetBool("Attack", true);
                enemy.m_AttackColdown = 2f;

               // enemy.transform.position = 
            }
        }
    }

    public override void Update()
    {
        // Nếu là Enemy, có thể tìm Player để chuyển sang Chasing
        if (character is Enemy enemy)
        {
            if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > 1f)
            {
                if (enemy.m_AttackColdown > 0)
                {
                    return;
                }
                m_StateMachine.SetState(CharacterStateID.Chasing);
            }
            else
            {
                enemy.m_AttackColdown = 2f;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"{typeof(T).Name} rời khỏi trạng thái Attack.");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.isStopped = false;
                enemy.m_Animator.SetBool("Attack", false);
            }
        }
    }

}
