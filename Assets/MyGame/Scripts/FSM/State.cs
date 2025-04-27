using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected BaseStateMachine<T> m_StateMachine;
    protected T m_Character;

    protected State(BaseStateMachine<T> stateMachine, T character)
    {
        m_StateMachine = stateMachine;
        m_Character = character;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
