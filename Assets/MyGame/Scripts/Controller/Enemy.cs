using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent m_Agent;
    public Enemy m_Enemy;
    public Animator m_Animator;
    [SerializeField]
    public EnemySO m_EnemyS0;

    private const string PLAYER_TAG = "Player";
    private float m_EnemySpeed = 0f;
    private Vector3 m_SpawnPoint = Vector3.zero;
    public float m_AttackColdown;
    private float m_Heath;
    private float m_MeleeDamage;
    private float m_RangeDamage;
    private GameStateID m_GameState;
    private Player m_Player;


    [SerializeField] private GameObject m_AttackPoint;
    private EnemyStateMachine m_StateMachine;
    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<EnemyStateMachine>();
        m_Enemy = GetComponent<Enemy>();
    }
    public Vector3 GetAttackPoint()
    {
        return m_AttackPoint.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialized();
        m_Player = FindFirstObjectByType<Player>();
        m_Agent.stoppingDistance = 2f;
        m_Agent.speed = m_EnemySpeed;
        m_SpawnPoint = gameObject.transform.position;
        m_StateMachine.AddState(CharacterStateID.Idle, new EnemyIdleState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Chasing, new EnemyChasingState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Attack, new EnemyAttackState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Death, new EnemyDeathState<Enemy>(m_StateMachine, this));
        m_StateMachine.SetState(CharacterStateID.Idle);

    }
    private void Initialized()
    {
        m_Heath = m_EnemyS0.m_Heath;
        m_AttackColdown = m_EnemyS0.m_AttackTime;
        m_MeleeDamage = m_EnemyS0.m_MeleeDamage;
        m_RangeDamage = m_EnemyS0.m_RangeDamage;
        m_EnemySpeed = m_EnemyS0.m_Speed;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsDead())
        {
            m_StateMachine.SetState(CharacterStateID.Death);
            return;
        }

        if (m_AttackColdown > 0)
        {
            m_AttackColdown -= Time.deltaTime;
        }
    }
    public bool IsDead()
    {
        if (m_Heath <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnAttack()
    {
        Collider[] all = Physics.OverlapSphere(m_AttackPoint.transform.position, 2f);
        if (all.Length > 0)
        {
            for (int i = 0; i < all.Length; i++)
            {
                if (all[i] != null && all[i].CompareTag("Player"))
                {
                    Debug.Log($"m_MeleeDamage : {m_MeleeDamage}");
                    m_Player.TakeDamage(m_MeleeDamage);
                }
            }
        }
    }
    public void OnHit()
    {
        if (m_Player != null)
        {
            m_Player.TakeDamage(m_MeleeDamage);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            OnHit();
        }
    }
    public Vector3 GetPlayerPos()
    {
        return m_Player.transform.position;
    }
    public void TakeDamage(float damage)
    {
        //Debug.Log($"damage : {damage}");
        if (damage > 0)
        {
            m_Heath -= damage;
            //Debug.Log($"m_Heath : {m_Heath}");
            if (m_Heath <= 0)
            {
                m_StateMachine.SetState(CharacterStateID.Death);
                ListenerManager.Instance.BroadCast(ListenType.ON_ENEMY_DEATH, "Won !!!");
            }
        }
    }
}
