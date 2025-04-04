using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BaseStateMachine<T> : MonoBehaviour
{
    private Dictionary<CharacterStateID, State<T>> states = new Dictionary<CharacterStateID, State<T>> ();
    private State<T> m_CurrentState;
    protected T m_Character;
    private void Awake()
    {
        m_Character = GetComponent<T>();
    }
    public void AddState(CharacterStateID key, State<T> state)
    {
        states[key] = state;
    }

    public void SetState(CharacterStateID newState)
    {
        if (m_CurrentState != null)
        {
            m_CurrentState.Exit();
        }

        if (states.TryGetValue(newState, out State<T> state))
        {
            m_CurrentState = state;
            m_CurrentState.Enter();
        }
        else
        {
            Debug.LogWarning($"State {newState} chưa được thêm vào StateMachine.");
        }
    }

    private void Update()
    {
        m_CurrentState?.Update();
    }
}
