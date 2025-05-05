using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : BaseScreen
{
    [SerializeField] Slider m_HPSlider;
    [SerializeField] Slider m_MPSlider;
    [SerializeField] Slider m_StaminaSlider;
    [SerializeField] TextMeshProUGUI m_Heathtxt;
    [SerializeField] TextMeshProUGUI m_Manatxt;
    [SerializeField] TextMeshProUGUI m_Staminatxt;

    [SerializeField] TextMeshProUGUI m_MissionName;
    [SerializeField] TextMeshProUGUI m_MisstionDes;
    [SerializeField] GameObject m_MisionProgress;
    private List<TMP_Text> m_MisionProgressViews = new();
    public override void Init()
    {
        base.Init();
        
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_MANA, OnUpdatePlayerManaEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_STAMINA, OnUpdatePlayerStaminaEvent);
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
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_MANA, OnUpdatePlayerManaEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_STAMINA, OnUpdatePlayerStaminaEvent);
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
        if (value != null)
        {
            if (value is Player currentPlayer)
            {
                m_Heathtxt.text = $"{currentPlayer.GetPlayerCurrentHeath().ToString()}/{currentPlayer.GetPlayerMaxHeath().ToString()}";
                m_Manatxt.text = $"{currentPlayer.GetPlayerCurrentMana().ToString()}/{currentPlayer.GetPlayerMaxMana().ToString()}";
                m_Staminatxt.text = $"{currentPlayer.GetPlayerCurrentStamina().ToString()}/{currentPlayer.GetPlayerMaxStamina().ToString()}";
                m_HPSlider.value = currentPlayer.GetPlayerCurrentHeath() / currentPlayer.GetPlayerMaxHeath();
                m_MPSlider.value = currentPlayer.GetPlayerCurrentMana() / currentPlayer.GetPlayerMaxMana();
                m_StaminaSlider.value = currentPlayer.GetPlayerCurrentStamina() / currentPlayer.GetPlayerMaxStamina();
            }
        }
    }
    private void OnUpdatePlayerHealthEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Heathtxt.text = $"{currentvalue.GetPlayerCurrentHeath().ToString()}/{currentvalue.GetPlayerMaxHeath().ToString()}";
                m_HPSlider.value = currentvalue.GetPlayerCurrentHeath() / currentvalue.GetPlayerMaxHeath();
            }
        }
    }
    private void OnUpdatePlayerManaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Manatxt.text = $"{currentvalue.GetPlayerCurrentMana().ToString()}/{currentvalue.GetPlayerMaxMana().ToString()}";
                m_MPSlider.value = currentvalue.GetPlayerCurrentMana() / currentvalue.GetPlayerMaxMana();
            }
        }
    }
    private void OnUpdatePlayerStaminaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Staminatxt.text = $"{currentvalue.GetPlayerCurrentStamina().ToString()}/{currentvalue.GetPlayerMaxStamina().ToString()}";
                m_StaminaSlider.value = currentvalue.GetPlayerCurrentStamina() / currentvalue.GetPlayerMaxStamina();
            }
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
