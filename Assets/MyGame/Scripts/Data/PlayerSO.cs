using UnityEngine;

[CreateAssetMenu(menuName = "ScripttableObject/Player", fileName = "PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [Header("Player")]
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