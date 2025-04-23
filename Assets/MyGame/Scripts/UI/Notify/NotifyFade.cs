using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class NotifyFade : BaseNotify
{
    public Image fadeImage;
    public Color fadeColor;

    public override void Init()
    {
        base.Init();
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void Fade(float fadeTime, Action onDuringFade, Action onFinish)
    {
        fadeImage.color = fadeColor;
        SetAlpha(0);
        Sequence seq = DOTween.Sequence();
        seq.Append(fadeImage.DOFade(1f, fadeTime));
        seq.AppendCallback(() => { onDuringFade?.Invoke(); });
        seq.Append(fadeImage.DOFade(0f, fadeTime));
        seq.OnComplete(() =>
        {
            onFinish?.Invoke();
            this.Hide();
        });
    }

    private void SetAlpha(float value)
    {
        Color cl = fadeImage.color;
        cl.a = value;
        fadeImage.color = cl;
    }
}
