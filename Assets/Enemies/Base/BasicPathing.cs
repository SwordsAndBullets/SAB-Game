using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPathing : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Entity playerEntity;
    private float playerDistance;

    [Header("Layers")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private LayerMask visualLayer;
    [SerializeField] private LayerMask playerLayer;

    private float groundLevel;
    private Ray checkRayCast;
    private Vector3 rayCastOffset = new Vector3(0, 1.0f, 0);

    [Header("States")]
    public int state = 0; //0-Patrolling, 1-Chasing, 2-Attacking

    [Header("0-Patrolling")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    private Vector3 movePoint;
    private Vector3 point;
    private bool movePointSet;

    [Header("1-Chasing")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float chaseSpeed;
    private Vector3 playerLastPosition;

    private void Update() {
        SetGroundLevel();
        playerDistance = Vector3.Distance(transform.position, player.position);
        try { state = (playerDistance <= detectionRange) ? ((playerDistance <= gameObject.GetComponent<Entity>().EquippedItem.distance) || (playerDistance <= 1)) ? 2 : 1 : 0; }
        catch { state = (playerDistance <= detectionRange) ? 1 : 0; }

        switch (state){
            case 0: Patrolling(); break;
            case 1: Chasing(); break;
            case 2: Attacking(); break;
        }
    }
    #region Gizmos
    private void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(movePoint, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        try{Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<Entity>().EquippedItem.distance);}
        catch{}
        switch (state){
            case 0: Gizmos.color = Color.green; Gizmos.DrawRay(checkRayCast); break;
            case 1: Gizmos.color = Color.blue; Gizmos.DrawRay(checkRayCast); break;
            case 2: Gizmos.color = Color.red; Gizmos.DrawRay(checkRayCast); break;
        }
    }
    #endregion

    #region Patrolling
    private void Patrolling(){
        if(CheckObstaclesInFront(3.0f)){
            Debug.Log("Obstacle; recalculating route.");
            movePointSet = false;
        }//Check for obstacles in front of it. Not needed when chasing as it only chases with direct line of sight and not needed when attacking as it stays still.

        if(Vector3.Distance(transform.position, movePoint) < 1f){
            movePointSet = false;
        }//Unset point to find a new walk point when within a certain range of it.

        if(!movePointSet){
            SetWalkPoint();
        }

        transform.LookAt(movePoint);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movePoint.x, groundLevel, movePoint.z), moveSpeed * Time.deltaTime);
        //Move
    }
    private void SetWalkPoint(){
        while(!movePointSet){
            float randomX = Random.Range((transform.position.x - moveRange), (transform.position.x + moveRange));//Gen Random X
            float randomZ = Random.Range((transform.position.z - moveRange), (transform.position.z + moveRange));//Gen Random Z
            RaycastHit hit;
            point = new Vector3(randomX, transform.position.y + 10f, randomZ);
            checkRayCast = new Ray(point, Vector3.down);
            Physics.Raycast(checkRayCast, out hit, Mathf.Infinity, walkableLayer);//Find Y Level of AI walkable ground
            try{ point = new Vector3(point.x, hit.transform.position.y + 0.1f, point.z); }
            catch{ int placeholderaakjsoiusdba = 0; }
            
            if(!Physics.CheckSphere(point, 1f, obstacleLayer) && Physics.CheckSphere(point, 1f, walkableLayer)){
                movePoint = point;
                movePointSet = true;
            }//Check if point inside obstacle
        }
    }
    private bool CheckObstaclesInFront(float range){
        bool result = Physics.Raycast(transform.position, GetDirectionTo(playerLastPosition), range, obstacleLayer);
        return result;
    }//Check for obstacles in obstacle layer in front of self by specified range.
    #endregion

    #region Chasing
    private void Chasing(){
        SetPlayerLastPosition();
        transform.position = Vector3.MoveTowards(transform.position, playerLastPosition, chaseSpeed * Time.deltaTime);
    }
    private void SetPlayerLastPosition(){
        RaycastHit hit;
        checkRayCast = new Ray(transform.position + rayCastOffset, GetDirectionTo(player.transform.position));
        Physics.Raycast(checkRayCast, out hit, 100.0f);
        try {
            if(hit.transform.name == player.name){
                playerLastPosition = player.transform.position;
                Debug.Log(playerLastPosition);
            }else{
                Debug.Log("No line of sight: " + hit.transform.name);
            }
        }
        catch { Debug.Log("No line of sight (Nothing hit)"); }
    }
    #endregion

    #region Attacking
    private void Attacking(){
        Debug.Log("Attacking");
        gameObject.GetComponent<Entity>().EquippedItem.Use(this.transform, gameObject.GetComponent<Entity>());
    }
    #endregion

    private void SetGroundLevel(){
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, out hit, Mathf.Infinity, walkableLayer);
        groundLevel = hit.point.y;
    }
    private Vector3 GetDirectionTo(Vector3 target){
        return (target - transform.position).normalized;
    }
}