using UnityEngine;

public class PopupStartGame : BasePopup
{

    public override void Show(object data)
    {
        base.Show(data);
    }
    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickStartGameButton()
    {
        if (UIManager.HasInstance)
        {
            this.Hide();
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.ShowNotify<NotifyLoadingGame>();
            
        }
    }
    public void OnClickSettingButton()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }    
    }
    public void OnClickExitButton()
    {
    #if UNITY_EDITOR
            // Nếu đang chạy trong Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        // Nếu đang chạy bản build (EXE, APK...)
        Application.Quit();
    #endif
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
