using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerController player;
    private Animator m_Animator;
    private const string PLAYER_TAG = "Player";
    private float m_EnemySpeed = 0f;
    private Vector3 m_SpawnPoint = Vector3.zero;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        agent.stoppingDistance = 1f;
        m_SpawnPoint = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (agent == null || player == null)
        {
            return;
        }
        SetMoveSpeed();
        if (Vector3.Distance(m_SpawnPoint, gameObject.transform.position) > 10 || !IsAimPlayer(player.transform.position))
        {
            agent.SetDestination(m_SpawnPoint);
            m_Animator.SetBool("swiping", false);
        }
        else
        {
            agent.SetDestination(player.transform.position);
            if (agent.remainingDistance <= agent.stoppingDistance || IsAnimationPlaying("swiping"))
            {
                m_Animator.SetBool("swiping", true);
                agent.isStopped = true;
            }
            else
            {
                m_Animator.SetBool("swiping", false);
                agent.isStopped = false;
            }
        }    
    }

    private bool IsAimPlayer(Vector3 tager)
    {
        if (Vector3.Distance(tager, gameObject.transform.position) > 5)
        {
            //Debug.Log(Vector3.Dot(gameObject.transform.position, tager));
            //if(Vector3.Dot(tager, gameObject.transform.position))
            return false;
        }
        else
        {
            return true;
        }    
    }    
    private void SetMoveSpeed()
    {
        if (agent.remainingDistance > 4f)
        {
            m_EnemySpeed = Mathf.Lerp(m_EnemySpeed, 2f, 1f);

        }
        else if (agent.remainingDistance > 2f)
        {
            m_EnemySpeed = Mathf.Lerp(m_EnemySpeed, 2f, 1f);
        }
        else
        {
            m_EnemySpeed = Mathf.Lerp(m_EnemySpeed, 0f, 1f);
        }
        m_Animator.SetFloat("Speed", m_EnemySpeed);
    }
    bool IsAnimationPlaying(string animationName)
    {
        // Kiểm tra trạng thái của animation hiện tại
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0); // Lấy thông tin trạng thái của layer 0

        // So sánh tên animation hiện tại với tên animation bạn muốn kiểm tra
        return stateInfo.IsName(animationName);
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    
    //    Debug.Log(other.tag.ToString());
    //    if (other.CompareTag(PLAYER_TAG))
    //    {
    //        m_Animator.SetBool("swiping", true);
    //    }
    //    //else
    //    //{
    //    //    m_Animator.SetBool("swiping", true);
    //    //}
    //}
}
