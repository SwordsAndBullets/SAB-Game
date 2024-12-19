using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPathing : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Entity playerEntity;

    [Header("Layers")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private LayerMask visualLayer;
    [SerializeField] private LayerMask playerLayer;

    private float groundLevel;

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
    private bool playerPosKnown = false;

    [Header("2-Attacking")]
    [SerializeField] private Item equippedItem;

    private void Update() {
        SetGroundLevel();
        switch (state){
            case 0: Patrolling(); break;
            case 1: Chasing(); break;
            case 2: Attacking(); break;
        }
    }

    #region Patrolling
    private void Patrolling(){
        if(CheckObstaclesInFront(3.0f)){
            Debug.Log("Obstacle; recalculating route.")
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
            Physics.Raycast(point, Vector3.down, out hit, Mathf.Infinity, walkableLayer);//Find Y Level of AI walkable ground
            point = new Vector3(point.x, hit.transform.position.y + 0.1f, point.z);
            
            if(!Physics.CheckSphere(point, 1f, obstacleLayer) && Physics.CheckSphere(point, 1f, walkableLayer)){
                movePoint = point;
                movePointSet = true;
            }//Check if point inside obstacle
        }
    }
    private void CheckObstaclesInFront(float range){
        Quaternion rotation = transfrom.rotation;
        transform.LookAt(playerPos);
        bool result = Physics.Raycast(transform.position.x, transform.rotation * Vector3.forward, range, obstacleLayer);
        transform.rotation = rotation;
        return result;
    }//Check for obstacles in obstacle layer in front of self by specified range.
    #endregion

    #region Chasing
    private void Chasing(){
        float playerDistance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2.0f) + Mathf.Pow(player.position.z - transform.position.z));//2D
        playerDistance = Mathf.Sqrt(Mathf.Pow(playerDistance) + Mathf.Pow(player.position.y - transform.position.y));//3D
        //Get Distance of player from enemy, use 3D pythagoras.

        if(playerDistance <= detectionRange){
            Quaternion rotation = transform.rotation;
            transform.LookAt(player);
            RaycastHit hit;
            bool lineOfSight = Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out hit, Mathf.Infinity);
            if(hit.transform.gameObject.layer == playerLayer){
                playerLastPosition = player.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, playerLastPosition, chaseSpeed * Time.deltaTime);
        }
    #endregion

    #region Attacking
    private void Attacking(){
        Debug.Log("Attacking");
        equippedItem.Use(playerEntity);
    }
    #endregion

    private void SetGroundLevel(){
        RaycastHit hit;
        Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, out hit, Mathf.Infinity, walkableLayer);
        groundLevel = hit.transform.position.y;
    }
}
