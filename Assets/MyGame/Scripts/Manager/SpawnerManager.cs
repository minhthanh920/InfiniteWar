using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    [System.Serializable]
    public class SpawnInfo
    {
        public string tag;
        public float spawnInterval = 2f;
        public int spawnCount = 1;
        public int maxSpawn = 10;
        [HideInInspector] public int currentSpawned = 0;
    }

    [Header("Danh sách các vị trí spawn")]
    public List<Transform> spawnPoints;

    [Header("Danh sách các loại spawn")]
    public List<SpawnInfo> spawnPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var spawnInfo in spawnPrefabs)
        {
            StartCoroutine(SpawnRoutine(spawnInfo));
        }
    }

    private IEnumerator SpawnRoutine(SpawnInfo info)
    {
        while (true)
        {
            for (int i = 0; i < info.spawnCount; i++)
            {
                if (info.currentSpawned >= info.maxSpawn)
                {
                    break;
                }    
                GameObject obj = PoolManager.Instance.SpawnFromPool(
                    info.tag,
                    spawnPoints[1].position,
                    spawnPoints[1].rotation
                );
                Debug.Log($"Object {obj.name} instantiated at position {obj.transform.position}");

                if (obj != null)
                {
                    info.currentSpawned++;
                }
            }

            yield return new WaitForSeconds(info.spawnInterval);
        }
    }
    /// <summary>
    /// Gọi khi enemy chết để giảm số lượng đang spawn
    /// </summary>
    public void OnEnemyDespawn(GameObject enemy)
    {
        foreach (var info in spawnPrefabs)
        {
            if (enemy.CompareTag(info.tag))
            {
                info.currentSpawned = Mathf.Max(0, info.currentSpawned - 1);
                break;
            }
        }
    }
}
