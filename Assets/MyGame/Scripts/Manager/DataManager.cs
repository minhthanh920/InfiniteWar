public class DataManager : BaseManager<DataManager>
{
    public GlobalConfig GlobalConfig;
    public float GetLoadingTime()
    {
        return GlobalConfig.LoadingTime;
    }
    
    public float GetFadeTime()
    {
        return GlobalConfig.FadeTime;
    }
    
    public int GetDeathCameraPriority()
    {
        return GlobalConfig.DeathCameraPriority;
    }
    public float GetEnemySpawnTime()
    {
        return GlobalConfig.EnemySpawnTime;
    }
}