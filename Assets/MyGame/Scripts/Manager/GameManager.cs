using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    void Start()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                scr.DoAnimationLoadingProgress(DataManager.Instance.GetLoadingTime(), () =>
                {
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    scr.Hide();
                });
            }
        }
    }

    public void StartGame()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_BMG_4);
        }

        Cursor.lockState = CursorLockMode.Locked;

        //if (MissionManager.HasInstance)
        //{
        //    MissionManager.Instance.ResetMission();
        //}
    }

    public void PauseGame()
    {

    }

    public void ResumeGame()
    {

    }

    public void RestartGame()
    {

    }

    public void EndGame()
    {

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (UIManager.HasInstance)
        {
            string txtMessage = "You Loose \n<size=50><#667986>Try again later";

            PopupShowMessageData data = new PopupShowMessageData(txtMessage, () =>
            {
                Time.timeScale = 1;
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                screenGame.Hide();

                UIManager.Instance.ShowNotify<NotifyFade>();
                NotifyFade notifyFade = UIManager.Instance.GetExistNotify<NotifyFade>();
                if (notifyFade != null)
                {
                    notifyFade.Fade(DataManager.Instance.GetFadeTime(),
                        onDuringFade: () =>
                        {
                            SceneManager.UnloadSceneAsync("Main");
                        },
                        onFinish: () =>
                        {
                            UIManager.Instance.ShowScreen<ScreenHome>();
                        });
                }
            });

            UIManager.Instance.ShowPopup<PopupShowMessage>(data, forceShowData: true);
        }

        //if (MissionManager.HasInstance)
        //{
        //    MissionManager.Instance.ResetMission();
        //}
    }

    public void WinGame()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.ON_WIN_GAME);
        }

        Time.timeScale = 0;
        if (UIManager.HasInstance)
        {
            string txtMessage = "Congratulation\n<size=50><#667986>You win!!!";

            PopupShowMessageData data = new PopupShowMessageData(txtMessage, () =>
            {
                Time.timeScale = 1;
                ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
                screenGame.Hide();

                UIManager.Instance.ShowNotify<NotifyFade>();
                NotifyFade notifyFade = UIManager.Instance.GetExistNotify<NotifyFade>();
                if (notifyFade != null)
                {
                    notifyFade.Fade(DataManager.Instance.GetFadeTime(),
                        onDuringFade: () =>
                        {
                            SceneManager.UnloadSceneAsync("Main");
                        },
                        onFinish: () =>
                        {
                            UIManager.Instance.ShowScreen<ScreenHome>();
                        });
                }
            });

            UIManager.Instance.ShowPopup<PopupShowMessage>(data, forceShowData: true);
        }

        //if (MissionManager.HasInstance)
        //{
        //    MissionManager.Instance.ResetMission();
        //}
    }
}
