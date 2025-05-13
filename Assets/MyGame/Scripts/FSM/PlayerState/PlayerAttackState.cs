using System.Collections;
using UnityEngine;

public class PlayerAttackState : State<Player>
{
    public PlayerAttackState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character){ }
    private float m_Progress;
    AnimatorStateInfo m_StateInfo;
    public override void Enter()
    {
        
        m_Character.m_Animator.SetBool(CONST.ATTACK, true);
        m_Character.StartCoroutine(DoDamage());
        m_Character.m_IsAttack = true;
    }
    public override void Update()
    {
        
        //m_StateInfo = m_Character.m_Animator.GetCurrentAnimatorStateInfo(0);
        //m_Progress = m_StateInfo.normalizedTime; // normalizedTime có thể >1 nếu loop
        //Debug.Log(m_Progress);
        //// Thoát khỏi AttackState khi animation kết thúc
        //if (m_Progress >= 1f)
        //{
        //    m_StateMachine.SetState(CharacterStateID.Idle);
        //}
    }    
    private IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.4f);
        m_Character.m_Weapon?.EnableDamage();
        m_Character.m_EffectPrefab?.Play();
        m_Character.AttackSound();
        yield return new WaitForSeconds(0.2f);
        m_Character.m_Weapon?.DisableDamage();
        yield return new WaitForSeconds(0.2f);
        m_StateMachine.SetState(CharacterStateID.Idle);
    }
    public override void Exit()
    {
        Debug.Log($" rời khỏi trạng thái Attack.");
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool(CONST.ATTACK, false);
        }
        if (m_Character.m_Weapon != null)
        {
            //player.m_Weapon.DisableWeapon();
        }
        if (m_Character.m_EffectPrefab != null)
        {
            m_Character.m_EffectPrefab.Stop();
        }
        m_Character.m_IsAttack = false;
        m_Character.m_Input.attack = false;
        m_Character.m_RootMotion = Vector3.zero;
    }
}
