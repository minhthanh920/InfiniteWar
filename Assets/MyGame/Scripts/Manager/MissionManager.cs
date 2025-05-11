using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionManager : BaseManager<MissionManager>
{
    public List<MissionSO> MissionData;

    private MissionSO currentMission;
    public MissionSO CurrentMission => currentMission;

    private int missionIndex = 0;
    public int MissionIndex => missionIndex;

    private int enemyDeadCount;

    private void Start()
    {
        currentMission = MissionData[missionIndex];
        ResetMission();
    }

    public void NextMission()
    {
        Debug.Log($"IsCompeteAllMissions : {IsCompeteAllMissions()}");

        if (!IsCompeteAllMissions())
        {
            if (currentMission.IsComplete)
            {
                missionIndex++;
                currentMission = MissionData[missionIndex];
                enemyDeadCount = 0;

                if (ListenerManager.HasInstance)
                {
                    ListenerManager.Instance.BroadCast(ListenType.UPDATE_MISSION, currentMission);
                }
            }
        }
        else
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.WinGame();
            }
        }
    }

    public bool IsCompeteAllMissions()
    {
        return MissionData.All(mission => mission.IsComplete);
    }

    public void CountEnemyDeath()
    {
        enemyDeadCount++;

        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_COUNT_ENEMY, enemyDeadCount);
        }

        if (enemyDeadCount >= currentMission.TotalEnemy)
        {
            currentMission.IsComplete = true;

            NextMission();
        }
    }

    public void ResetMission()
    {
        foreach (var mission in MissionData)
        {
            mission.IsComplete = false;
        }
    }
    public MissionSO GetCurrentMission()
    {
        return currentMission;
    }    
    public int GetKilledEnemy()
    {
        return enemyDeadCount;
    }    
}
