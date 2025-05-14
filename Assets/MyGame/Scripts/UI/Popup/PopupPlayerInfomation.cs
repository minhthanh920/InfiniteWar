using TMPro;
using UnityEngine;

public class PopupPlayerInfomation : BasePopup
{
    [SerializeField] TextMeshProUGUI m_AttackDamage;
    [SerializeField] TextMeshProUGUI m_HP;
    [SerializeField] TextMeshProUGUI m_MP;
    [SerializeField] TextMeshProUGUI m_SP;
    public override void Show(object data)
    {
        base.Show(data);
        if (GameManager.HasInstance)
        {
            m_AttackDamage.text = GameManager.Instance.GetPlayer().GetDamage().ToString();
            m_HP.text = $"{GameManager.Instance.GetPlayer().GetCurrentHeath().ToString()}/{GameManager.Instance.GetPlayer().GetMaxHeath().ToString()}";
            m_MP.text = $"{GameManager.Instance.GetPlayer().GetCurrentMana().ToString()}/{GameManager.Instance.GetPlayer().GetMaxMana().ToString()}";
            m_MP.text = $"{GameManager.Instance.GetPlayer().GetCurrentMana().ToString()}/{GameManager.Instance.GetPlayer().GetMaxMana().ToString()}";
        }

    }
    private void OnEnable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_MANA, OnUpdatePlayerManaEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_STAMINA, OnUpdatePlayerStaminaEvent);
            ListenerManager.Instance.Register(ListenType.UPDATE_PLAYER_DAMAGE, OnUpdatePlayerDamageEvent);
        }
        if (GameManager.HasInstance) 
        {
            m_AttackDamage.text = GameManager.Instance.GetPlayer().GetDamage().ToString();
            m_HP.text = $"{GameManager.Instance.GetPlayer().GetCurrentHeath().ToString()}/{GameManager.Instance.GetPlayer().GetMaxHeath().ToString()}";
            m_MP.text = $"{GameManager.Instance.GetPlayer().GetCurrentMana().ToString()}/{GameManager.Instance.GetPlayer().GetMaxMana().ToString()}";
            m_MP.text = $"{GameManager.Instance.GetPlayer().GetCurrentMana().ToString()}/{GameManager.Instance.GetPlayer().GetMaxMana().ToString()}";
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (!isHide)
            {
                this.Hide();
            }
            else
            {
                this.Show(this);
            }
            return;
        }
    }
    private void OnUpdatePlayerDamageEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_AttackDamage.text = $"{currentvalue.GetDamage().ToString()}";
            }
        }
    }
    private void OnUpdatePlayerHealthEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_HP.text = $"{currentvalue.GetCurrentHeath().ToString()}/{currentvalue.GetMaxHeath().ToString()}";
            }
        }
    }

    private void OnUpdatePlayerManaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_MP.text = $"{currentvalue.GetCurrentMana().ToString()}/{currentvalue.GetMaxMana().ToString()}";
            }
        }
    }
    private void OnUpdatePlayerStaminaEvent(object value)
    {
        if (value != null)
        {
            if (value is Player currentvalue)
            {
                m_SP.text = $"{currentvalue.GetCurrentStamina().ToString()}/{currentvalue.GetMaxStamina().ToString()}";
            }
        }
    }
    public override void Hide()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_HEALTH, OnUpdatePlayerHealthEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_MANA, OnUpdatePlayerManaEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_STAMINA, OnUpdatePlayerStaminaEvent);
            ListenerManager.Instance.Unregister(ListenType.UPDATE_PLAYER_DAMAGE, OnUpdatePlayerDamageEvent);
        }
        base.Hide();

    }
    public override void OnPlaySoundClickButton()
    {
        base.OnPlaySoundClickButton();
    }

    public override void OnPlaySoundHoverButton()
    {
        base.OnPlaySoundHoverButton();
    }
}
