using System.Collections;
using UnityEngine;

public class PlayerSkillBState : State<Player>
{
    public PlayerSkillBState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    private float m_Progress;
    AnimatorStateInfo m_StateInfo;
    public override void Enter()
    {
        m_Character.m_Animator.SetBool(CONST.SKILL_B, true);
        m_Character.StartCoroutine(DoDamage());
        m_Character.RemainMana(m_Character.m_SkillBCost);
    }
    public override void Update()
    {
    }
    private IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.6f);
        m_Character.m_Weapon?.EnableDamage(m_Character.GetSkillDamage(2));
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
            m_Character.m_Animator.SetBool(CONST.SKILL_B, false);
        }
        m_Character.m_IsUseSkillB = false;
        m_Character.m_RootMotion = Vector3.zero;
    }
}
