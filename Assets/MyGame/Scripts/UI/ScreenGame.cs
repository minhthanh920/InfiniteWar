using TMPro;
using UnityEngine;

public class ScreenGame : BaseScreen
{
    [SerializeField] TMP_Text m_HPText;
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
        m_HPText.text = "100";
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
        }
    }
    private void OnDisable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_USER_INFO, OnUpdateUserInfoEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
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
                m_HPText.text = currentHealth.ToString();
            }
        }
    }
}
