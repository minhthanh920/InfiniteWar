using UnityEngine;
public class PatrollingState : AIState
{
    public PatrollingState(AIStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Bắt đầu tuần tra...");
    }

    public override void Update()
    {
        if (Vector3.Distance(m_AIStateMachine.transform.position, PlayerController.Instance.transform.position) < 5f)
        {
            m_AIStateMachine.SetState(AiStateID.Patrol);
        }
    }

    public override void Exit()
    {
        Debug.Log("Dừng tuần tra...");
    }
}
