using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Collider m_WeaponCollider;
    private Player m_Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_WeaponCollider = GetComponent<Collider>();
        m_Player = GetComponentInParent<Player>();
        m_WeaponCollider.enabled = false; // Tắt mặc định
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableWeapon()
    {
        m_WeaponCollider.enabled = true;
    }
    public void DisableWeapon()
    {
        m_WeaponCollider.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Gây sát thương địch
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(m_Player.GetDamage()); // Tuỳ bạn set damage
            }
        }
    }
}
