using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChasingState<T> : State<T>
{
    private Animator m_Animator;
    private NavMeshAgent m_Agent;
    public ChasingState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character) 
    {
        if (character is MonoBehaviour mb)
        {
            m_Animator = mb.GetComponent<Animator>();
            m_Agent = mb.GetComponent<NavMeshAgent>();
        }
    }

    public override void Enter()
    {
        Debug.Log($"{typeof(T).Name} bắt đầu ĐUỔI THEO!");
        if (m_Animator != null)
        {
            m_Animator.SetBool("Run", true);
        }
    }

    public override void Update()
    {

        if(Player.Instance.m_AttackTime > 0)
        {
            m_Agent.SetDestination(Vector3.zero);
            return;
        }
        if (character is Enemy enemy)
        {
            if (Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > 10f)
            {
                m_StateMachine.SetState(CharacterStateID.Idle);
            }
            else if(Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) <=2f)
            {
                //m_Agent.isStopped = true;
                Player.Instance.m_AttackTime = 2f;
                m_Agent.SetDestination(Vector3.zero);
                m_StateMachine.SetState(CharacterStateID.Attack);
            }
            else
            {
                //m_Agent.isStopped = false;
                m_Agent.SetDestination(Player.Instance.transform.position);
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"{typeof(T).Name} ngừng đuổi theo.");
        if (m_Animator != null)
        {
            m_Animator.SetBool("Run", false);
        }
    }
}
