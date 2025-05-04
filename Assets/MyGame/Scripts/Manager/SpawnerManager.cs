using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        public string tag;               // tag dùng để lấy prefab từ PoolManager
        public float spawnInterval = 2f; // thời gian giữa mỗi lần spawn
        public int spawnCount = 1;       // số lượng spawn mỗi lần
        public int maxSpawn = 10;
        [HideInInspector] public int currentSpawned = 0;
    }

    [Header("Danh sách các vị trí spawn")]
    public List<Transform> spawnPoints;

    [Header("Danh sách các loại spawn")]
    public List<SpawnInfo> spawnPrefabs;

    private void Start()
    {
        foreach (var spawnInfo in spawnPrefabs)
        {
            StartCoroutine(SpawnRoutine(spawnInfo));
        }
    }

    private IEnumerator SpawnRoutine(SpawnInfo info)
    {
        while (info.currentSpawned < info.maxSpawn)
        {
            for (int i = 0; i < info.spawnCount; i++)
            {
                if (info.currentSpawned >= info.maxSpawn)
                    break;

                Transform spawnPoint = GetRandomSpawnPoint();
                if (spawnPoint != null)
                {
                    Vector3 spawnPos = GetNearestNavMeshPoint(spawnPoint.position);
                    GameObject obj = PoolManager.Instance.SpawnFromPool(
                        info.tag,
                        spawnPos,
                        spawnPoint.rotation
                    );

                    if (obj != null)
                        info.currentSpawned++;
                }
            }

            yield return new WaitForSeconds(info.spawnInterval);
        }
    }

    private Vector3 GetNearestNavMeshPoint(Vector3 position)
    {
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas))
        {
            return hit.position;
        }
        return position; // fallback nếu không tìm được
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0) return null;
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
