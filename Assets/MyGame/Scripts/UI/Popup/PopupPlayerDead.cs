using UnityEngine;

public class PopupPlayerDead : BasePopup
{
    public override void Show(object data)
    {
        base.Show(data);
    }
    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickPlayAgianButton()
    {
        this.Hide();
        if (GameManager.HasInstance)
        {
            GameManager.Instance.RestartGame();
        }
        
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
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
