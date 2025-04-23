public enum GameStateID
{
    Unknow,
    Start,
    End,
    Pause,
    GameOver,
}
public enum PlayerStateID
{
    None,
    Walk,
    Run,
    Attack,
    Jump,
    Die,
    WalkBack
}
public enum ListenType
{
    ANY = 0,
    ON_PLAYER_DEATH,
    ON_ENEMY_DEATH,
    UPDATE_COUNT_TEXT,
    UPDATE_USER_INFO,
    UPDATE_PLAYER_HEALTH,
    ON_WIN_GAME,
    UPDATE_MISSION,
    UPDATE_COUNT_ENEMY,
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
}

public enum WeaponSlot
{
    Primary = 0,
    Secondary = 1
}

public enum SocketID
{
    RightLeg,
    RightHand
}

public enum EquipWeaponBy
{
    Player,
    AI
}