public class EnemyStateMachine : BaseStateMachine<Enemy>
{
    protected override void Awake()
    {
        base.Awake();
        AddStates();
        SetState(CharacterStateID.Idle);
    }
    private void AddStates()
    {
        AddState(CharacterStateID.Idle, new EnemyIdleState(this, m_Character));
        AddState(CharacterStateID.Attack, new EnemyAttackState(this, m_Character));
        AddState(CharacterStateID.Walk, new EnemyWalkState(this, m_Character));
        AddState(CharacterStateID.Run, new EnemyRunState(this, m_Character));
        AddState(CharacterStateID.Death, new EnemyDeathState(this, m_Character));
        AddState(CharacterStateID.Chasing, new EnemyChasingState(this, m_Character));
    }
    public void ResetState()
    {
        SetState(CharacterStateID.Idle);
    }
}
