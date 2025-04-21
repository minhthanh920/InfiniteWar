using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenGame : BaseScreen
{
    [SerializeField] Slider m_HPSlider;
    [SerializeField] GameObject m_GameOver;
    [SerializeField] TMP_Text m_Text;
    public override void Init()
    {
        base.Init();

    }
    public override void Hide()
    {
        base.Hide();

    }
    public override void Show(object data)
    {
        base.Show(data);
    }
    private void Start()
    {
        m_GameOver.SetActive(false);
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Register(ListenType.ON_PLAYER_DEATH, OnPlayerDeathEvent);
        }
    }
    private void OnDisable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Unregister(ListenType.ON_PLAYER_DEATH, OnPlayerDeathEvent);
        }
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
    }
    private void OnPlayerDeathEvent(object value)
    {
       
        if (value != null)
        {
            if (value is string text)
            {
                Debug.Log($"text : {text}");
                m_Text.text = text;
                m_GameOver.SetActive(true);
            }
        }
    }
}
