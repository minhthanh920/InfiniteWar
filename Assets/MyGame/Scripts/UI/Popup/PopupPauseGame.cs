using UnityEngine;

public class PopupPauseGame : BasePopup
{

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
        if (Input.GetKeyDown(KeyCode.Escape))
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
    public override void Hide()
    {
        base.Hide();
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.GetPlayer() != null)
            {
                GameManager.Instance.GetPlayer().RestoreMouseSpeed();
            }
        }
    }

    public void OnClickResumeButton()
    {
        this.Hide();
    }
    public void OnClickSettingButton()
    {
        this.Hide();
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
