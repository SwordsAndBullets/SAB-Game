using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private Transform TPlayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;

    [Header("Patrolling")]
    [SerializeField] private float walkPointRange;
    Vector3 walkPoint;
    bool walkPointSet;

    [Header("Attacking")]
    [SerializeField] private float attackSpeed;
    bool attacked;
    
    [Header("Spotting")]
    [SerializeField] private float sightRange;
    [SerializeField] private float attackRange;
    bool playerInSightRange;
    bool playerInAttackRange;

    private void Start() {
        TPlayer = GameObject.Find("Player").transform;
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        playerInSightRange = Physics.Raycast(transform.position, TPlayer.position - transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.Raycast(transform.position, TPlayer.position - transform.position, attackRange, playerLayer);

        if(playerInAttackRange){
            Attacking();
        }else if(playerInSightRange){
            Chasing();
        }else{
            Patrolling();
        }
    }

    //States
    private void Patrolling(){
        if(!walkPointSet){ FindWalkPoint(); }
        else{ Agent.SetDestination(walkPoint); }

        //Reset walk point
        Vector3 distanceToWalkPoint = walkPoint - transform.position;
        if(distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
    private void Chasing(){
        Agent.SetDestination(TPlayer.position);
    }
    private void Attacking(){
        Debug.Log("Enemy Attacking...");
        //Attack Function Here
        Agent.SetDestination(transform.position);
        transform.LookAt(TPlayer);
        if(!attacked){
            attacked = true;
            Invoke(nameof(ResetAttack), attackSpeed);
        }
    }

    //Functions
    private void FindWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(randomX, transform.position.x, randomZ);
        if(Physics.Raycast(walkPoint, -transform.up, 0.2f, groundLayer)){ walkPointSet = true; }
    }
    private void ResetAttack(){
        attacked = false;
    }
}
