using UnityEngine;

public class PopupCheatGame : BasePopup
{
    public override void Show(object data)
    {
        base.Show(data);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.Hide();
        }
    }
    public override void Hide()
    {
        base.Hide();
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
