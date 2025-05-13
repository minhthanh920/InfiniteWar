using UnityEngine;

public class PlayerStateMachine : BaseStateMachine<Player>
{
    protected override void Awake()
    {
        base.Awake();
        AddStates();
        SetState(CharacterStateID.Idle);
    }

    private void AddStates()
    {
        AddState(CharacterStateID.Idle, new PlayerIdleState(this, m_Character));
        AddState(CharacterStateID.Walk, new PlayerWalkState(this, m_Character));
        AddState(CharacterStateID.Run, new PlayerRunState(this, m_Character));
        AddState(CharacterStateID.Jump, new PlayerJumpState(this, m_Character));
        AddState(CharacterStateID.Attack, new PlayerAttackState(this, m_Character));
        AddState(CharacterStateID.HeavyAttack, new PlayerHeavyAttackState(this, m_Character));
        AddState(CharacterStateID.Death, new PlayerDeathState(this, m_Character));
    }
}
