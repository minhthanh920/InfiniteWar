using UnityEngine;

[CreateAssetMenu(menuName = "ScripttableObject/Enemy", fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    [Header("Enemy")]
    //public Animator m_Animator;
    //public CharacterController m_CharacterController;
    //public Avatar m_Avatar;
    public float m_Speed;
    public float m_Heath;
    public float m_Mana;
    public float m_Stamina;
    public float m_MeleeDamage;
    public float m_RangeDamage;
}