using UnityEngine;

public class PopupTutorials : BasePopup
{
    private Player m_Player;
    public override void Show(object data)
    {
        base.Show(data);

    }
    private void Update()
    {
        if (base.isHide)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Hide();
            return;
        }
    }
    public void OnCloseButton()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupTutorials>();
            if (GameManager.HasInstance)
            {
                if (GameManager.Instance.GetPlayer() != null)
                {
                    GameManager.Instance.GetPlayer().RestoreMouseSpeed();
                }
            }
        }
        this.Hide();
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
