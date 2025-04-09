using UnityEngine;

public class IdleState<T> : State<T>
{
    public IdleState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character) 
    {
    }
    

    public override void Enter()
    {
        Debug.Log($"{typeof(T).Name} vào trạng thái IDLE.");
        if (character is Enemy enemy)
        {
            enemy.m_Agent.isStopped = true;
            enemy.m_Agent.SetDestination(Vector3.zero);
            enemy.m_Animator.SetBool("Idle", true);
        }
    }

    public override void Update()
    {
        // Nếu là Enemy, có thể tìm Player để chuyển sang Chasing
        if (character is Enemy enemy)
        {
            if(Vector3.Distance(enemy.GetAttackPoint(), Player.Instance.transform.position) <= 0)
            {
                //Player.Instance.m
            }
            if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) < 5f)
            {
                if (Player.Instance.m_AttackTime > 0)
                {
                    return;
                }
                m_StateMachine.SetState(CharacterStateID.Chasing);
            }
            else if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) <= 2f)
            {
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
            //else
            //{
            //    m_Agent.SetDestination(Player.Instance.transform.position);
            //}
        }
    }

    public override void Exit()
    {
        Debug.Log($"{typeof(T).Name} rời khỏi trạng thái IDLE.");
        if (character is Enemy enemy)
        {
            enemy.m_Animator.SetBool("Idle", false);
        }
    }
}

