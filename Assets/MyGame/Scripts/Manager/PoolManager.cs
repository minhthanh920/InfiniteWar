using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnSpawned();
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string m_Tag;
        public GameObject m_Prefab;
        public int m_Size = 10;
        public bool m_CanExpand = true; // Mặc định cho phép mở rộng pool
    }

    [Header("Danh sách các Pool")]
    public List<Pool> m_Pools;

    private Dictionary<string, Queue<GameObject>> m_ObjectPools;
    private Dictionary<string, Pool> m_PoolSettings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        m_ObjectPools = new Dictionary<string, Queue<GameObject>>();
        m_PoolSettings = new Dictionary<string, Pool>();

        foreach (var pool in m_Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.m_Size; i++)
            {
                GameObject obj = Instantiate(pool.m_Prefab);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            m_ObjectPools.Add(pool.m_Tag, objectPool);
            m_PoolSettings.Add(pool.m_Tag, pool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, float autoReturnTime = 0f)
    {
        if (!m_ObjectPools.ContainsKey(tag))
        {
            Debug.LogWarning($"Không tìm thấy Pool với tag: {tag}");
            return null;
        }

        Queue<GameObject> poolQueue = m_ObjectPools[tag];

        if (poolQueue.Count == 0)
        {
            // Nếu Pool trống và được phép mở rộng
            if (m_PoolSettings.ContainsKey(tag) && m_PoolSettings[tag].m_CanExpand)
            {
                GameObject newObj = Instantiate(m_PoolSettings[tag].m_Prefab);
                newObj.transform.SetParent(this.transform);
                newObj.SetActive(false);
                poolQueue.Enqueue(newObj);
            }
            else
            {
                Debug.LogWarning($"Pool {tag} đã hết object và không thể mở rộng!");
                return null;
            }
        }

        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // Nếu object có IPoolable, gọi OnSpawned()
        var poolable = objectToSpawn.GetComponent<IPoolable>();
        poolable?.OnSpawned();

        // Reset Particle nếu có
        var particle = objectToSpawn.GetComponent<ParticleSystem>();
        if (particle != null)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play();
        }

        // Auto Return nếu có thời gian
        if (autoReturnTime > 0f)
        {
            StartCoroutine(ReturnAfterSeconds(objectToSpawn, autoReturnTime, tag));
        }

        return objectToSpawn;
    }

    public void ReturnToPool(GameObject obj, string tag)
    {
        obj.SetActive(false);

        if (!m_ObjectPools.ContainsKey(tag))
        {
            Debug.LogWarning($"Không tìm thấy Pool khi return với tag: {tag}");
            Destroy(obj);
            return;
        }

        m_ObjectPools[tag].Enqueue(obj);
    }

    private System.Collections.IEnumerator ReturnAfterSeconds(GameObject obj, float seconds, string tag)
    {
        yield return new WaitForSeconds(seconds);
        ReturnToPool(obj, tag);
    }
}
