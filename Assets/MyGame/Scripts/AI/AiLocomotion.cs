using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (navMeshAgent.hasPath)
        {
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }
}
