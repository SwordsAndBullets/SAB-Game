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
    private Vector3 playerPos = new Vector3(0,0,0);
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
        if(!movePointSet){
            SetWalkPoint();
        }

        transform.LookAt(movePoint);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(movePoint.x, groundLevel, movePoint.z), moveSpeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movePoint) < 1f){
            movePointSet = false;
        }
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
    #endregion

    #region Chasing
    private void Chasing(){
        if(Physics.CheckSphere(transform.position, detectionRange, playerLayer)){
            playerPos = player.transform.position;
            playerPosKnown = true;
        }else{
            playerPosKnown = false;
        }
        if(playerPosKnown){
            transform.LookAt(playerPos);
            switch (GetPlayerLineOfSight(playerPos)){
                case true:
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, groundLevel, playerPos.z), chaseSpeed * Time.deltaTime);
                    break;
                case false:
                    Patrolling();
                    break;
            }
        }else{
            state = 0;
            Patrolling();
        }
    }
    private bool GetPlayerLineOfSight(Vector3 playerPosition){
        return Physics.Raycast(transform.position, transform.rotation * Vector3.forward, Mathf.Infinity, visualLayer);
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
