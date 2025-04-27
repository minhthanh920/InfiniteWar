using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Cài đặt Effect")]
    //public string m_EffectGround = "Impact_Ground";
    public string m_EffectWall = "Impact_Wall";
    public string m_EffectEnemy = "Impact_Enemy";

    [Header("Tag kiểm tra")]
    public string m_EnemyTag = "Enemy";

    [Header("Thông số thêm")]
    public float m_EffectLifetime = 0.1f; // thời gian tự hủy effect (nếu cần)

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
        RaycastHit hit;
        Vector3 direction = (other.transform.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            SpawnImpactEffect(hit.point, hit.normal);
        }
        if (!other.CompareTag("Enemy")) return;
        if (m_AlreadyHit.Contains(other)) return;
        m_AlreadyHit.Add(other);
        other.GetComponent<Enemy>()?.TakeDamage(m_Player.GetDamage());

    }

    public string effectTag = "Ground";  // Cái này có thể thay đổi thành "Wall" hoặc các tag khác
    public float spawnRate = 0.1f;      // Tần suất spawn hiệu ứng (0.1s mỗi lần)

    private float lastSpawnTime;

    private void OnTriggerStay(Collider other)
    {
        // Kiểm tra nếu va chạm với mặt đất, tường hay vật thể có tag "Ground" hoặc "Wall"
        //if (other.CompareTag(effectTag))
        //{
            // Nếu đã đủ thời gian spawn lại effect (chống spam)
            if (Time.time - lastSpawnTime >= spawnRate)
            {
                // Lấy điểm va chạm và hướng pháp tuyến
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = (transform.position - other.transform.position).normalized;

                // Spawn hiệu ứng tia lửa tại điểm va chạm và xoay theo hướng pháp tuyến
                SpawnImpactEffect(hitPoint, hitNormal);

                // Cập nhật thời gian spawn effect mới
                lastSpawnTime = Time.time;
            }
        //}

    }
    private void SpawnImpactEffect(Vector3 m_Position, Vector3 m_Normal)
    {
        // Lấy effect từ Pool
        GameObject m_Effect = PoolManager.Instance.SpawnFromPool(m_EffectWall, m_Position, Quaternion.LookRotation(m_Normal), m_EffectLifetime);

        if (m_Effect != null)
        {
            // Nếu muốn random nhẹ hướng, thêm chút xoay
            //m_Effect.transform.rotation = Quaternion.LookRotation(m_Normal) * Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            // Option: tự disable sau 0.5s nếu bạn không muốn pooling quản lý lifetime
            // StartCoroutine(DisableAfterSeconds(m_Effect, 0.5f));
        }
    }
}
