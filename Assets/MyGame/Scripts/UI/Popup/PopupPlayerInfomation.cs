using UnityEngine;

public class PopupPlayerInfomation : BasePopup
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
    public override void OnPlaySoundClickButton()
    {
        base.OnPlaySoundClickButton();
    }

    public override void OnPlaySoundHoverButton()
    {
        base.OnPlaySoundHoverButton();
    }
}
