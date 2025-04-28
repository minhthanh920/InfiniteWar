using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NotifyLoadingGame : BaseNotify
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    public override void Init()
    {
        base.Init();
        StartCoroutine(LoadScene());
    }

    public override void Show(object data)
    {
        base.Show(data);
        StartCoroutine(LoadScene());
    }

    public override void Hide()
    {
        base.Hide();
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DesertTown", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText("Press the space bar to continue");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ShowNotify<NotifyFade>();
                        NotifyFade notifyFade = UIManager.Instance.GetExistNotify<NotifyFade>();
                        if (notifyFade != null)
                        {
                            notifyFade.Fade(DataManager.Instance.GetFadeTime(),
                                onDuringFade: () =>
                                {
                                    asyncOperation.allowSceneActivation = true;
                                },
                                onFinish: () =>
                                {
                                    
                                    if (GameManager.HasInstance)
                                    {
                                        this.Hide();
                                        UIManager.Instance.ShowScreen<ScreenGame>();
                                        GameManager.Instance.StartGame();
                                        
                                    }
                                });
                        }
                    }


                }
            }
            yield return null;
        }
    }
}
