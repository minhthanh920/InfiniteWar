using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
    [SerializeField] Image m_Skill1;
    [SerializeField] Image m_Skill2;
    [SerializeField] Image m_Skill3;
    [SerializeField] Image m_Skill4;
    [SerializeField] Image m_Skill5;
    private Color m_Skill1Color;
    private Color m_Skill2Color;
    private Color m_Skill3Color;
    private Color m_Skill4Color;
    private Color m_Skill5Color;
    private List<TMP_Text> m_MisionProgressViews = new();
    public override void Init()
    {
        base.Init();
        m_Skill1Color = m_Skill1.color;
        m_Skill2Color = m_Skill2.color;
        m_Skill3Color = m_Skill3.color;
        m_Skill4Color = m_Skill4.color;
        m_Skill5Color = m_Skill5.color;
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_MANA, OnUpdatePlayerManaEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_STAMINA, OnUpdatePlayerStaminaEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_MISSION, OnUpdateMissionEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_USE_SKILL, OnUseSkill);
            //ListenerManager.Instance.Register(ListenType.ON_PLAYER_DEATH, OnPlayerDeathEvent);
            //ListenerManager.Instance.Register(ListenType.ON_ENEMY_DEATH, OnEnemyDeathEvent);
        }
        InitMission();
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupTutorials>();
            if (GameManager.HasInstance)
            {
                if (GameManager.Instance.GetPlayer() != null)
                {
                    GameManager.Instance.GetPlayer().SetMouseSpeed(0);
                }
            }
        }

    }
    private void OnUseSkill(object value)
    {
        if(value == null)
        {
            return;
        }    
        if(value is int nvalue)
        {
            if (nvalue == 1)
            {
                m_Skill1.color = Color.black;
                
            }
            else if (nvalue == 2)
            {
                m_Skill2.color = Color.black;
            }
            else if (nvalue == 3)
            {
                m_Skill3.color = Color.black;
            }
            else if (nvalue == 4)
            {
                m_Skill4.color = Color.black;
            }
            else
            {
                m_Skill5.color = Color.black;
            }
            StartCoroutine(ColdDown(nvalue));
        }    
    }
    private IEnumerator ColdDown(int value)
    {
        yield return new WaitForSeconds(1f);
        if(value == 1)
        {
            m_Skill1.color = m_Skill1Color;
        }
        if (value == 2)
        {
            m_Skill2.color = m_Skill2Color;
        }
        if (value == 3)
        {
            m_Skill3.color = m_Skill3Color;
        }
        if (value == 4)
        {
            m_Skill4.color = m_Skill4Color;
        }
        if (value == 5)
        {
            m_Skill5.color = m_Skill5Color;
        }

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
            ListenerManager.Instance.Unregister(ListenType.UPDATE_USE_SKILL, OnUseSkill);
            //ListenerManager.Instance.Unregister(ListenType.ON_PLAYER_DEATH, OnEnemyDeathEvent);
        }
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.GetPlayer() != null)
            {
                GameManager.Instance.GetPlayer().RestoreMouseSpeed();
            }
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
                m_Heathtxt.text = $"{currentPlayer.GetCurrentHeath().ToString()}/{currentPlayer.GetMaxHeath().ToString()}";
                m_Manatxt.text = $"{currentPlayer.GetCurrentMana().ToString()}/{currentPlayer.GetMaxMana().ToString()}";
                m_Staminatxt.text = $"{currentPlayer.GetCurrentStamina().ToString()}/{currentPlayer.GetMaxStamina().ToString()}";
                m_HPSlider.value = currentPlayer.GetCurrentHeath() / currentPlayer.GetMaxHeath();
                m_MPSlider.value = currentPlayer.GetCurrentMana() / currentPlayer.GetMaxMana();
                m_StaminaSlider.value = currentPlayer.GetCurrentStamina() / currentPlayer.GetMaxStamina();
            }
        }
    }
    private void OnUpdatePlayerHealthEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Heathtxt.text = $"{currentvalue.GetCurrentHeath().ToString()}/{currentvalue.GetMaxHeath().ToString()}";
                m_HPSlider.value = currentvalue.GetCurrentHeath() / currentvalue.GetMaxHeath();
            }
        }
    }
    private void OnUpdatePlayerManaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Manatxt.text = $"{currentvalue.GetCurrentMana().ToString()}/{currentvalue.GetMaxMana().ToString()}";
                m_MPSlider.value = currentvalue.GetCurrentMana() / currentvalue.GetMaxMana();
            }
        }
    }
    private void OnUpdatePlayerStaminaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_Staminatxt.text = $"{currentvalue.GetCurrentStamina().ToString()}/{currentvalue.GetMaxStamina().ToString()}";
                m_StaminaSlider.value = currentvalue.GetCurrentStamina() / currentvalue.GetMaxStamina();
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
