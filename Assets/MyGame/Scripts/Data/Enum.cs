public enum GameStateID
{
    Unknow,
    Start,
    End,
    Pause,
    GameOver,
}
public enum ListenType
{
    ANY = 0,    
    ON_PLAYER_DEATH,
    ON_ENEMY_DEATH,
    UPDATE_COUNT_TEXT,
    UPDATE_USER_INFO,
    UPDATE_PLAYER_HEALTH,
    UPDATE_PLAYER_MANA,
    UPDATE_PLAYER_STAMINA,
    ON_WIN_GAME,
    UPDATE_MISSION,
    UPDATE_COUNT_ENEMY,
    ON_PAUSE_GAME,
    ON_RESUME_GAME,
    UPDATE_PLAYER_DAMAGE,
    UPDATE_USE_SKILL,
}

public enum UIType
{
    Unknow = 0,
    Screen = 1,
    Popup = 2,
    Notify = 3,
    Overlap = 4,
}
public enum CharacterStateID
{
    Walk,
    Run,
    Jump,
    Death,
    Idle,
    Attack,
    Chasing,
    HeavyAttack,
    SkillA,
    SkillB,
    SkillC,
    SkillD,
    SkillI,
}
