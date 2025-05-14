using UnityEngine;

public class ScreenHome : BaseScreen
{
    public override void Show(object data)
    {
        base.Show(data);
        UIManager.Instance.ShowPopup<PopupStartGame>();
        Cursor.visible = true; // Hiển thị con trỏ chuột
        Cursor.lockState = CursorLockMode.None; // Không khóa con trỏ chuột
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickPopupSetting()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
    }

    public void StartGame()
    {
        //Debug.Log("Click start game");
        if (UIManager.HasInstance)
        {
            this.Hide();
            UIManager.Instance.ShowNotify<NotifyLoadingGame>();
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
