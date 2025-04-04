using UnityEngine;

public class AttackState<T> : State<T>
{
    private Animator m_Animator;
    public AttackState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
        if (character is MonoBehaviour mb)
        {
            m_Animator = mb.GetComponent<Animator>();
        }
    }


    public override void Enter()
    {
        Debug.Log($"{typeof(T).Name} vào trạng thái Attack.");
        if (m_Animator != null)
        {
            m_Animator.SetBool("Attack", true);
        }
    }

    public override void Update()
    {
        // Nếu là Enemy, có thể tìm Player để chuyển sang Chasing
        if (character is Enemy enemy)
        {
            if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > 2f)
            {
                if (Player.Instance.m_AttackTime > 0)
                {
                    return;
                }
                m_StateMachine.SetState(CharacterStateID.Chasing);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"{typeof(T).Name} rời khỏi trạng thái Attack.");
        if (m_Animator != null)
        {
            m_Animator.SetBool("Attack", false);
        }
    }
}
