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
        AddState(CharacterStateID.SkillA, new PlayerSkillAState(this, m_Character));
        AddState(CharacterStateID.SkillB, new PlayerSkillBState(this, m_Character));
        AddState(CharacterStateID.SkillC, new PlayerSkillCState(this, m_Character));
        AddState(CharacterStateID.SkillD, new PlayerSkillDState(this, m_Character));
        AddState(CharacterStateID.SkillI, new PlayerSkillIState(this, m_Character));
    }
}
