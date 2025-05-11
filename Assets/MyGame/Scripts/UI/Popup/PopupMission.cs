using TMPro;
using UnityEngine;

public class PopupMission : BasePopup
{
    [SerializeField] TextMeshProUGUI m_MissionName;
    [SerializeField] TextMeshProUGUI m_MisstionDes;
    [SerializeField] TextMeshProUGUI m_MisstionAim;
    [SerializeField] GameObject m_MisionProgress;
    [SerializeField] 
    public override void Show(object data)
    {
        base.Show(data);

    }
    public override void Hide()
    {
        base.Hide();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Hide();
        }
    }
    private void OnEnable()
    {
        if (MissionManager.HasInstance)
        {
            m_MissionName.text = MissionManager.Instance.GetCurrentMission().MissionName;
            m_MisstionDes.text = MissionManager.Instance.GetCurrentMission().MissionDes;
            m_MisstionAim.text = $"{MissionManager.Instance.GetKilledEnemy().ToString()}/{MissionManager.Instance.CurrentMission.TotalEnemy.ToString()}";
        }
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
        }
    }
    private void OnDisable()
    {
        ListenerManager.Instance.Register(ListenType.UPDATE_COUNT_ENEMY, OnUpdateCountEnemyEvent);
    }
    private void OnUpdateCountEnemyEvent(object value)
    {
        if (value is int)
        {
            m_MisstionAim.text = $"{value}/{MissionManager.Instance.CurrentMission.TotalEnemy.ToString()}";
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
