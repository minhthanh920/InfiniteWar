using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private GameStateID m_GameStateID = GameStateID.Unknow;
    private void Start()
    {
        m_GameStateID = GameStateID.Start;
        if (UIManager.HasInstance)
        {
            //UIManager.Instance.ShowNotify<NotifyLoading>();
            //NotifyLoading scr = UIManager.Instance.GetExistNotify<NotifyLoading>();
            //if (scr != null)
            //{
            //    scr.AnimationLoaddingText();
            //    scr.DoAnimationLoadingProgress(5, () =>
            //    {
            //        UIManager.Instance.ShowScreen<ScreenHome>();
            //        scr.Hide();
            //    });
            //}
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public GameStateID GetGameState()
    {
        return m_GameStateID;
    }
    public void SetGameState(GameStateID gameStateID)
    {
        m_GameStateID = gameStateID;
    }
}