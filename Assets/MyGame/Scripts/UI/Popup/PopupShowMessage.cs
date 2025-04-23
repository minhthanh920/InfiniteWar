using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class PopupShowMessage : BasePopup
{
    [SerializeField]
    private TMP_Text TxtMessage;
    [SerializeField]
    private Button BtnBackToMenu;

    private Action btnAction;

    public override void Init()
    {
        base.Init();
        BtnBackToMenu?.onClick.AddListener(OnClickBtnBackToMenu);
    }

    public override void Show(object data)
    {
        base.Show(data);

        if (data != null)
        {
            if (data is PopupShowMessageData msgData)
            {
                TxtMessage.text = msgData.TxtMessage;
                btnAction = msgData.OnClickEvent;
            }
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

    private void OnClickBtnBackToMenu()
    {
        btnAction?.Invoke();
        this.Hide();
    }
}
