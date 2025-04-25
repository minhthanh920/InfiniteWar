using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Collider m_WeaponCollider;
    private Player m_Player;
    private bool m_CanDamage;
    private HashSet<Collider> m_AlreadyHit = new HashSet<Collider>(); // Tránh trúng 1 địch nhiều lần
    void Awake()
    {
        m_WeaponCollider = GetComponent<Collider>();
        m_Player = GetComponentInParent<Player>();
        
    }
    void Start()
    {
        m_WeaponCollider.enabled = false;
    }
    public void EnableDamage()
    {
        m_WeaponCollider.enabled = true;
        m_CanDamage = true;
    }
    public void DisableDamage()
    {
        m_WeaponCollider.enabled = false;
        m_CanDamage = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (m_AlreadyHit.Contains(other)) return;
        m_AlreadyHit.Add(other);
        other.GetComponent<Enemy>()?.TakeDamage(m_Player.GetDamage());
    }
}
