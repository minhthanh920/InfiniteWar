using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : BaseScreen
{
    [SerializeField] Slider m_HPSlider;
    [SerializeField] Slider m_MPSlider;
    [SerializeField] Slider m_StaminaSlider;
    [SerializeField] TextMeshProUGUI m_MissionName;
    [SerializeField] TextMeshProUGUI m_MisstionDes;
    [SerializeField] GameObject m_MisionProgress;
    private List<TMP_Text> m_MisionProgressViews = new();
    public override void Init()
    {
        base.Init();
        //m_HPSlider.value = 1f;
        //m_MPSlider.value = 1f;
        //m_StaminaSlider.value = 1f;
        
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_MISSION, OnUpdateMissionEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
            //ListenerManager.Instance.Register(ListenType.ON_PLAYER_DEATH, OnPlayerDeathEvent);
            //ListenerManager.Instance.Register(ListenType.ON_ENEMY_DEATH, OnEnemyDeathEvent);
        }
        InitMission();

    }
    public override void Hide()
    {

        base.Hide();
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_MISSION, OnUpdateMissionEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
            //ListenerManager.Instance.Unregister(ListenType.ON_PLAYER_DEATH, OnEnemyDeathEvent);
        }

    }
    public override void Show(object data)
    {
        base.Show(data);
    }
    private void OnUpdateUserInfoEvent(object value)
    {
        //if (value != null)
        //{
        //    if (value is float currentHealth)
        //    {
        //        Debug.Log($"currentHealth : {currentHealth}");
        //        m_HPText.text = currentHealth.ToString();
        //    }
        //}

    }
    private void OnUpdatePlayerHealthEvent(object value)
    {
        if (value != null)
        {
            if (value is float currentHealth)
            {
                //Debug.Log($"currentHealth : {currentHealth}");
                m_HPSlider.value = currentHealth;
            }
        }
        else
        {
            m_HPSlider.value = 1;
        }
    }

    private void InitMission()
    {
        if (MissionManager.HasInstance)
        {
            for (int i = 0; i < MissionManager.Instance.MissionData.Count; i++)
            {
                GameObject missonGo = Instantiate(m_MisionProgress, m_MisionProgress.transform.parent);
                missonGo.SetActive(true);
                TMP_Text txtProgress = missonGo.GetComponent<TMP_Text>();
                txtProgress.text = $"Kill: 0 / {MissionManager.Instance.MissionData[i].TotalEnemy}";
                m_MisionProgressViews.Add(txtProgress);
            }
            m_MissionName.text = MissionManager.Instance.CurrentMission.MissionName;
            m_MisstionDes.text = MissionManager.Instance.CurrentMission.MissionDes;
        }
        else
        {
            Debug.Log("MissionManager.HasInstance");
        }
    }
    private void OnUpdateMissionEvent(object value)
    {
        if (value != null)
        {
            if (value is int countEnemyDead)
            {
                if (m_MisionProgressViews?.Count > 0)
                {
                    m_MisionProgressViews[MissionManager.Instance.MissionIndex].text = $"Kill: {countEnemyDead} / {MissionManager.Instance.CurrentMission.TotalEnemy}";
                }
            }
        }

    }
    private void OnUpdateCountEnemyEvent(object value)
    {
        if (value is MissionSO currentMission)
        {
            m_MissionName.text = currentMission.MissionName;
        }
    }
}
