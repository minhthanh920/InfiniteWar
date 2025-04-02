using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    private Dictionary<AiStateID, AIState> states = new Dictionary<AiStateID, AIState>();
    private AIState currentState;

    public void AddState(AiStateID key, AIState state)
    {
        states[key] = state;
    }

    public void SetState(AiStateID newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        if (states.TryGetValue(newState, out AIState state))
        {
            currentState = state;
            currentState.Enter();
        }
        else
        {
            Debug.LogWarning($"State {newState} chưa được thêm vào StateMachine.");
        }
    }

    private void Update()
    {
        currentState?.Update();
    }
}
