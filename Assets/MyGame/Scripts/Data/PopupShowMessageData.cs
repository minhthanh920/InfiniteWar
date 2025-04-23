using System;
public class PopupShowMessageData
{
    public string TxtMessage;
    public Action OnClickEvent;

    public PopupShowMessageData(string txtMessage, Action onClickEvent = null)
    {
        TxtMessage = txtMessage;
        OnClickEvent = onClickEvent;
    }
}
