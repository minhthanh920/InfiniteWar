public abstract class AIState
{
    protected AIStateMachine m_AIStateMachine;

    public AIState(AIStateMachine stateMachine)
    {
        this.m_AIStateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}