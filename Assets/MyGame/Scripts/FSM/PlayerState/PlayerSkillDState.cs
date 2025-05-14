using System.Collections;
using UnityEngine;

public class PlayerSkillDState : State<Player>
{
    public PlayerSkillDState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    private float m_Progress;
    AnimatorStateInfo m_StateInfo;
    public override void Enter()
    {
        m_Character.m_Animator.SetBool(CONST.SKILL_D, true);
        m_Character.StartCoroutine(DoDamage());
        m_Character.RemainMana(m_Character.m_SkillDCost);
    }
    public override void Update()
    {
    }
    private IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.5f);
        m_Character.m_Weapon?.EnableDamage(m_Character.GetSkillDamage(4));
        m_Character.m_EffectPrefab?.Play();
        m_Character.AttackSound();
        yield return new WaitForSeconds(1f);
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
            m_Character.m_Animator.SetBool(CONST.SKILL_D, false);
        }
        m_Character.m_IsUseSkillD = false;
        m_Character.m_RootMotion = Vector3.zero;
    }
}
