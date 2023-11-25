using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PersonController : MonoBehaviour
{
    Animator animator;
    [SerializeField] private Vector3 destination;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float stopDistance=0.05f;
    public bool reachedDestination= false;
    public bool destinationSet = false;
    public float distanceEd = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(0);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 destinationDirection = destination - transform.position;
        float distance = destinationDirection.magnitude;
        distanceEd = distance;
        if (distance <= stopDistance)
        {
            reachedDestination = true;
        }

    }

    public void SetDestination(Vector3 destination)
    {
        
        this.destination = destination;
        agent.destination = destination;
        reachedDestination = false;

    }
}
