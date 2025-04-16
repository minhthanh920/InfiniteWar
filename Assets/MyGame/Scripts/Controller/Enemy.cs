using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent m_Agent;
    public Enemy m_Enemy;
    public Animator m_Animator;
    [SerializeField]
    private EnemySO m_EnemyS0;
    private const string PLAYER_TAG = "Player";
    private float m_EnemySpeed = 0f;
    private Vector3 m_SpawnPoint = Vector3.zero;
    public float m_AttackColdown;
    private float m_Heath;
    private float m_MeleeDamage;
    private float m_RangeDamage;

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
        m_SpawnPoint = gameObject.transform.position;
        m_StateMachine.AddState(CharacterStateID.Idle, new IdleState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Chasing, new ChasingState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Attack, new AttackState<Enemy>(m_StateMachine, this));
        m_StateMachine.SetState(CharacterStateID.Idle);

    }
    private void Initialized()
    {
        m_Heath = m_EnemyS0.m_Heath;
        m_MeleeDamage = m_EnemyS0.m_MeleeDamage;
        m_RangeDamage = m_EnemyS0.m_RangeDamage;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_AttackColdown > 0)
        {
            m_AttackColdown -= Time.deltaTime;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            OnAttack();
        }
    }
}
