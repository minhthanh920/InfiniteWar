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
                    scr.Hide();
                    UIManager.Instance.ShowScreen<ScreenHome>();
                    
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
        if(ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.UPDATE_USER_INFO);
        }
        if (MissionManager.HasInstance)
        {
            MissionManager.Instance.ResetMission();
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ShowPopup<PopupSetting>();
        }
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResumeGame()
    {
        if (ListenerManager.HasInstance)
        {
            ListenerManager.Instance.BroadCast(ListenType.ON_RESUME_GAME);
        }
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
                            SceneManager.UnloadSceneAsync("DesertTown");
                        },
                        onFinish: () =>
                        {
                            UIManager.Instance.ShowScreen<ScreenHome>();
                        });
                }
            });

            UIManager.Instance.ShowPopup<PopupShowMessage>(data, forceShowData: true);
        }

        if (MissionManager.HasInstance)
        {
            MissionManager.Instance.ResetMission();
        }
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

        if (MissionManager.HasInstance)
        {
            MissionManager.Instance.ResetMission();
        }
    }
}
