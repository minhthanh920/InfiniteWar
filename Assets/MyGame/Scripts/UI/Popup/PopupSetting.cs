using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : BasePopup
{
    private float bgmVolume;
    private float effectVolume;
    public Slider sliderBGM;
    public Slider sliderEffect;

    public override void Show(object data)
    {
        base.Show(data);
        bgmVolume = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
        effectVolume = PlayerPrefs.GetFloat(CONST.SE_VOLUME_KEY, CONST.SE_VOLUME_DEFAULT);
        sliderBGM.value = bgmVolume;
        sliderEffect.value = effectVolume;
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickCloseButton()
    {
        this.Hide();
    }

    public void OnBGMValueChange(float v)
    {
        bgmVolume = v;
    }

    public void OnEffectValueChange(float v)
    {
        effectVolume = v;
    }

    public void OnApplySetting()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeSEVolume(effectVolume);
            AudioManager.Instance.ChangeBGMVolume(bgmVolume);
        }
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
