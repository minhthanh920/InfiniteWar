using UnityEngine;

public class PopupCheatGame : BasePopup
{
    private Player m_Player;
    public override void Show(object data)
    {
        base.Show(data);
   
    }
    private void OnEnable()
    {
        if (GameManager.HasInstance)
        {
            m_Player = GameManager.Instance.GetPlayer();
        }
    }
    private void Update()
    {
        if(base.isHide)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Hide();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            m_Player.AddDamage(1000);
            return;
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            m_Player.RestoreFull();
            return;
        }
    }
    public override void Hide()
    {
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
