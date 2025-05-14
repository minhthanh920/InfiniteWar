using TMPro;
using UnityEngine;

public class PopupMission : BasePopup
{
    [SerializeField] TextMeshProUGUI m_MissionName;
    [SerializeField] TextMeshProUGUI m_MisstionDes;
    [SerializeField] TextMeshProUGUI m_MisstionAim;
    [SerializeField] GameObject m_MisionProgress;
    public override void Show(object data)
    {
        if (MissionManager.HasInstance)
        {
            m_MissionName.text = MissionManager.Instance.GetCurrentMission().MissionName;
            m_MisstionDes.text = MissionManager.Instance.GetCurrentMission().MissionDes;
            m_MisstionAim.text = $"Tiêu Diệt : {MissionManager.Instance.GetKilledEnemy().ToString()}/{MissionManager.Instance.CurrentMission.TotalEnemy.ToString()}";
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
        }
        base.Show(data);
    }
    public override void Hide()
    {
        base.Hide();

    }
    private void OnEnable()
    {
        if (MissionManager.HasInstance)
        {
            m_MissionName.text = MissionManager.Instance.GetCurrentMission().MissionName;
            m_MisstionDes.text = MissionManager.Instance.GetCurrentMission().MissionDes;
            m_MisstionAim.text = $"Tiêu Diệt : {MissionManager.Instance.GetKilledEnemy().ToString()}/{MissionManager.Instance.CurrentMission.TotalEnemy.ToString()}";
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
        }
    }
    private void OnDisable()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Unregister(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
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
    private void OnUpdateCountEnemyEvent(object value)
    {
        if (value is int)
        {
            m_MisstionAim.text = $"Tiêu Diệt : {value}/{MissionManager.Instance.CurrentMission.TotalEnemy.ToString()}";
            m_MissionName.text = MissionManager.Instance.GetCurrentMission().MissionName;
            m_MisstionDes.text = MissionManager.Instance.GetCurrentMission().MissionDes;
        }
    }
    public void OnClickCloseButton()
    {
        this.Hide();
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
