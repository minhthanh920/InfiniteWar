using UnityEditor;
using UnityEngine;

public class PlayerAttackState : State<Player>
{
    public PlayerAttackState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character){ }
    private float m_Progress;
    AnimatorStateInfo m_StateInfo;
    public override void Enter()
    {
        m_Character.m_Animator.SetBool("Attack", true);
        if (m_Character.m_EffectPrefab != null)
        {
            m_Character.m_EffectPrefab.Play();
        }
    }

    public override void Update()
    {
        m_StateInfo = m_Character.m_Animator.GetCurrentAnimatorStateInfo(0);
        m_Progress = m_StateInfo.normalizedTime % 1f; // normalizedTime có thể >1 nếu loop

        //Debug.Log($"[AttackState] Progress: {m_Progress:F2}");

        // Kích hoạt Damage khi animation tới nửa đoạn
        if (m_Progress >= 0.5f)
        {
            m_Character.m_Weapon?.EnableDamage();

        }

        // Disable Damage nếu animation restart (loop lại)
        if (m_Progress < 0.5f)
        {
            m_Character.m_Weapon?.DisableDamage();
        }

        // Thoát khỏi AttackState khi animation kết thúc
        if (m_StateInfo.normalizedTime >= 1f)
        {
            m_Character.m_Animator.SetBool("Attack", false); // Tắt trigger Attack
            m_StateMachine.SetState(CharacterStateID.Idle);  // Chuyển về Idle
        }
    }

    public override void Exit()
    {
        //Debug.Log($" rời khỏi trạng thái Attack.");
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("Attack", false);
        }
        if (m_Character.m_Weapon != null)
        {
            //player.m_Weapon.DisableWeapon();
        }
        if (m_Character.m_EffectPrefab != null)
        {
            m_Character.m_EffectPrefab.Stop();
        }
        m_Character.m_Input.attack = false;
    }
}
