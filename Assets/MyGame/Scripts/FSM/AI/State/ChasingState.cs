using UnityEngine;

public class ChasingState : AIState
{
    public ChasingState(AIStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Bắt đầu đuổi theo!");
    }

    public override void Update()
    {
        m_AIStateMachine.transform.position = Vector3.MoveTowards(
            m_AIStateMachine.transform.position,
            PlayerController.Instance.transform.position,
            3f * Time.deltaTime
        );

        if (Vector3.Distance(m_AIStateMachine.transform.position, PlayerController.Instance.transform.position) > 10f)
        {
            m_AIStateMachine.SetState(AiStateID.ChasePlayer);
        }
    }

    public override void Exit()
    {
        Debug.Log("Ngừng đuổi theo!");
    }
}
