using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent m_Agent;
    public Enemy m_Enemy;
    public Animator m_Animator;
    private const string PLAYER_TAG = "Player";
    private float m_EnemySpeed = 0f;
    private Vector3 m_SpawnPoint = Vector3.zero;
    public float m_AttackColdown;

    private EnemyStateMachine m_StateMachine;
    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_StateMachine = GetComponent<EnemyStateMachine>();
        m_Enemy = GetComponent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Enemy = FindFirstObjectByType<Enemy>();
        m_Agent.stoppingDistance = 2f;
        m_SpawnPoint = gameObject.transform.position;
        m_StateMachine.AddState(CharacterStateID.Idle, new IdleState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Chasing, new ChasingState<Enemy>(m_StateMachine, this));
        m_StateMachine.AddState(CharacterStateID.Attack, new AttackState<Enemy>(m_StateMachine, this));
        m_StateMachine.SetState(CharacterStateID.Idle);

    }

    // Update is called once per frame
    void Update()
    {
        if (m_AttackColdown > 0)
        {
            m_AttackColdown -= Time.deltaTime;
        }
    }  

}
