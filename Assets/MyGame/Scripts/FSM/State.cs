public abstract class State<T>
{
    protected BaseStateMachine<T> m_StateMachine;
    protected T character;

    public State(BaseStateMachine<T> stateMachine, T character)
    {
        this.m_StateMachine = stateMachine;
        this.character = character;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}