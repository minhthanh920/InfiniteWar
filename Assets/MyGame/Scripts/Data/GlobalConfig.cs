using UnityEngine;

[CreateAssetMenu(menuName = "ScripttableObject/GlobalConfig", fileName ="GlobalConfig")]
public class GlobalConfig : ScriptableObject
{
    [Header("AI")]
    public float maxTime = 0.5f;
    public float maxDistance = 1f;
    public float aiMaxHealth = 100f;
    public float blinkDuration = 0.1f;
    public float dieForce = 5f;
    public float maxSight = 5f;
    public float timeDestroyAI = 2f;
    public float pickupWeaponSpeed = 5f;

    [Header("Player")]
    public float jumpHeight = 10f;
    public float gravity = 20f;
    public float stepDown = 0.1f;
    public float airControl = 2.5f;
    public float jumpDamp = 0.5f;
    public float groundSpeed = 1.2f;
    public float pushPower = 2f;
    public float turnSpeed = 15f;

    [Header("UI")]
    public float LoadingTime;
    public float FadeTime;

    [Header("Camera")]
    public int DeathCameraPriority;
    [Header("Enemy")]
    public float EnemySpawnTime = 5f;
}
