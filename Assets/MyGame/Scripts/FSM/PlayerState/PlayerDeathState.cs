using UnityEngine;

public class PlayerDeathState : State<Player>
{
    public PlayerDeathState(BaseStateMachine<Player> stateMachine, Player character) : base(stateMachine, character) { }
    public override void Enter()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", true);
        }
        m_Character.m_Collider.enabled = false;
        m_Character.m_Weapon.enabled = false;
        if(UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupPlayerDead>();
        }
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        if (m_Character.m_Animator != null)
        {
            m_Character.m_Animator.SetBool("IsDead", false);
        }
    }
}
