using System.Collections;
using UnityEngine;

public class PlayerSkillCState : State<Player>
{
    public PlayerSkillCState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    private float m_Progress;
    AnimatorStateInfo m_StateInfo;
    public override void Enter()
    {
        m_Character.m_Animator.SetBool(CONST.SKILL_C, true);
        m_Character.StartCoroutine(DoDamage());
        m_Character.RemainStamina(m_Character.m_SkillCCost);
    }
    public override void Update()
    {
    }
    private IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.9f);
        m_Character.m_Weapon?.EnableDamage();
        m_Character.m_EffectPrefab?.Play();
        m_Character.AttackSound();
        yield return new WaitForSeconds(0.3f);
        m_Character.m_Weapon?.DisableDamage();
        m_Character.m_EffectPrefab.Stop();
        yield return new WaitForSeconds(0.4f);
        m_StateMachine.SetState(CharacterStateID.Idle);
    }
    public override void Exit()
    {
        //Debug.Log($" rời khỏi trạng thái Attack.");
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool(CONST.SKILL_C, false);
        }
        m_Character.m_IsUseSkillC = false;
        m_Character.m_RootMotion = Vector3.zero;
    }
}
