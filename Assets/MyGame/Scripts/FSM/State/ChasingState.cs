using UnityEngine;
public class ChasingState<T> : State<T>
{
    public ChasingState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character) 
    {
        //if (character is MonoBehaviour mb)
        //{
        //    m_Animator = mb.GetComponent<Animator>();
        //    m_Agent = mb.GetComponent<NavMeshAgent>();
        //}
    }

    public override void Enter()
    {
        //Debug.Log($"{typeof(T).Name} bắt đầu ĐUỔI THEO!");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.isStopped = false;
                enemy.m_Animator.SetBool("Run", true);
            }
        }
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
        if (character is Enemy enemy)
        {
            //Debug.Log(enemy.m_AttackColdown);
            if (enemy.m_AttackColdown > 0f)
            {
                enemy.m_Agent.isStopped = true;
                enemy.m_Agent.SetDestination(Vector3.zero);
                return;
            }
            //else if(Vector3.Distance(enemy.transform.position, enemy.GetPlayerPos()) > 10f)
            //{
            //    m_StateMachine.SetState(CharacterStateID.Idle);
            //}
            else if(Vector3.Distance(enemy.transform.position, enemy.GetPlayerPos()) <=2f)
            {
                //m_Agent.isStopped = true;
                //Player.Instance.m_AttackTime = 2f;
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
            else
            {
               enemy.m_Agent.SetDestination(enemy.GetPlayerPos());
            }
        }
    }

    public override void Exit()
    {
        //Debug.Log($"{typeof(T).Name} ngừng đuổi theo.");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Animator.SetBool("Run", false);
                enemy.m_Agent.isStopped = true;
                enemy.m_Agent.SetDestination(Vector3.zero);

            }
        }
    }
}
