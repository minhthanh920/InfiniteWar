using UnityEngine;
using UnityEngine.Windows;

public class AttackState<T> : State<T>
{
    public AttackState(BaseStateMachine<T> stateMachine, T character) : base(stateMachine, character)
    {
        //if (character is MonoBehaviour mb)
        //{
        //    m_Animator = mb.GetComponent<Animator>();
        //}
    }
    public override void Enter()
    {
        Debug.Log($"{typeof(T).Name} vào trạng thái Attack.");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.isStopped = true;
                enemy.m_Agent.SetDestination(Vector3.zero);
                enemy.m_Animator.SetBool("Attack", true);
                enemy.m_AttackColdown = 2f;

               // enemy.transform.position = 
            }
        }
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("Attack", true);
                if(player.m_Weapon != null)
                {
                    player.m_Weapon.EnableWeapon();
                }    
            }
        }
    }

    public override void Update()
    {
        // Nếu là Enemy, có thể tìm Player để chuyển sang Chasing
        if (character is Enemy enemy)
        {
            if (Vector3.Distance(enemy.transform.position, enemy.GetPlayerPos()) > 1.5f)
            {
                if (enemy.m_AttackColdown > 0)
                {
                    return;
                }
                m_StateMachine.SetState(CharacterStateID.Chasing);
            }
            else
            {
                enemy.m_AttackColdown = 2f;
            }
        }
        if ( character is Player player )
        {
            //player.m_StateInfo = player.m_Animator.GetCurrentAnimatorStateInfo(0);
            //if(player.m_StateInfo.IsName("Attack"))
            //{
            //    player.m_Animator.SetBool("Attack1", true);
            //}
            //else if (player.m_StateInfo.IsName("Attack1"))
            //{
            //    player.m_Animator.SetBool("Attack2", true);
            //}
        }    
    }

    public override void Exit()
    {
        Debug.Log($"{typeof(T).Name} rời khỏi trạng thái Attack.");
        if (character is Enemy enemy)
        {
            if (enemy.m_Animator != null)
            {
                enemy.m_Agent.isStopped = false;
                enemy.m_Animator.SetBool("Attack", false);
            }
        }
        if (character is Player player)
        {
            if (player.m_Animator != null)
            {
                player.m_Animator.SetBool("Attack", false);
            }
            if (player.m_Weapon != null)
            {
                player.m_Weapon.DisableWeapon();
            }
        }
    }
}
