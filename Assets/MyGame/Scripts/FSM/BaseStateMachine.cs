using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine<T> : MonoBehaviour where T : MonoBehaviour
{
    protected T m_Character;
    private Dictionary<CharacterStateID, State<T>> m_States = new Dictionary<CharacterStateID, State<T>>();
    private State<T> m_CurrentState;
    public CharacterStateID m_CurrentStateID { get; private set; }

    protected virtual void Awake()
    {
        m_Character = GetComponent<T>();
        if (m_Character == null)
            Debug.LogError($"[BaseStateMachine] Không tìm thấy {typeof(T).Name} trên {gameObject.name}!");
    }

    public void AddState(CharacterStateID id, State<T> state)
    {
        if (!m_States.ContainsKey(id))
            m_States.Add(id, state);
    }

    public void SetState(CharacterStateID newState)
    {
        if (m_CurrentState != null)
            m_CurrentState.Exit();

        if (m_States.TryGetValue(newState, out var state))
        {
            m_CurrentStateID = newState;
            m_CurrentState = state;
            m_CurrentState.Enter();
            
        }
        else
        {
            Debug.LogWarning($"State {newState} chưa được thêm!");
        }
    }

    private void Update()
    {
        m_CurrentState?.Update();
    }
}
