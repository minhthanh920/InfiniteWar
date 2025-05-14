using UnityEngine;

public class PopupTutorials : BasePopup
{
    private Player m_Player;
    public override void Show(object data)
    {
        base.Show(data);
        if (UIManager.HasInstance)
        {
            if (GameManager.HasInstance)
            {
                if (GameManager.Instance.GetPlayer() != null)
                {
                    GameManager.Instance.GetPlayer().SetMouseSpeed(0);
                }
            }
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
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
    public void OnCloseButton()
    {
        this.Hide();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.SetGameState(GameStateID.Start);
        }
    }
    public override void Hide()
    {

        base.Hide();
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.GetPlayer() != null)
            {
                GameManager.Instance.GetPlayer().RestoreMouseSpeed();
                GameManager.Instance.SetGameState(GameStateID.Start);
            }
        }
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
