using UnityEngine;

public class IdleState<T> : State<T>
{
    public IdleState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character) 
    {
    }
    

    public override void Enter()
    {
        
        if (character is Enemy enemy)
        {
            enemy.m_Agent.isStopped = true;
            enemy.m_Agent.SetDestination(Vector3.zero);
            enemy.m_Animator.SetBool("Idle", true);
            //Debug.Log($"{typeof(T).Name} vào trạng thái IDLE.");
        }
        if (character is Player player)
        {

            player.m_Animator.SetBool("Idle", true);
            //Debug.Log($"{typeof(T).Name} vào trạng thái IDLE.");
        }
    }

    public override void Update()
    {
        // Nếu là Enemy, có thể tìm Player để chuyển sang Chasing
        if (character is Enemy enemy)
        {

            if (Vector3.Distance(enemy.transform.position, enemy.GetPlayerPos()) <= 1.5f)
            {
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
            else
            {
                if (enemy.m_AttackColdown > 0)
                {
                    return;
                }
                m_StateMachine.SetState(CharacterStateID.Chasing);
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log($"{typeof(T).Name} rời khỏi trạng thái IDLE.");
        if (character is Enemy enemy)
        {
            enemy.m_Animator.SetBool("Idle", false);
        }
        if (character is Player player)
        {
            player.m_Animator.SetBool("Idle", false);
        }
    }
}

