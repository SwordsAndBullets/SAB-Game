using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPathing : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private LayerMask visualLayer;
    [SerializeField] private LayerMask playerLayer;

    private float groundLevel;

    [Header("States")]
    private int state = 0; //0-Patrolling, 1-Chasing, 2-Attacking

    [Header("Patrolling")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    private Vector3 movePoint;
    private Vector3 point;
    private bool movePointSet;

    [Header("Chasing")]
    [SerializeField] private float detectionRange;
    [SerializeField] private float chaseSpeed;
    private Vector3 playerLastPosition;

    private void Update() {
        SetGroundLevel();
        switch (state){
            case 0: Patrolling(); break;
            case 1: Chasing(); break;
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
        Vector3 playerPos = GetPlayerPosition();
        if(playerPos != null){
            switch (GetPlayerLineOfSight()){
                case true:
                    transform.LookAt(playerPos);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, groundLevel, playerPos.z), chaseSpeed * Time.deltaTime);
                    break;
                case false:
                    Debug.Log("No Player Line of Sight");
                    Patrolling();
            }
            Debug.Log("No Player in Range");
            state = 0;
            Patrolling();
        }
    }
    private Vector3 GetPlayerPosition(){
        RaycastHit detectedPlayer;
        if(Physics.CheckSphere(transform.position, Vector3.up, out detectedPlayer, detectionRange, playerLayer)){
            return detectedPlayer.transform.position;
        }else{
            return null;
        }
    }
    private bool GetPlayerLineOfSight(Vector3 playerPosition){
        return Physics.Raycast(transform.position, transform.rotation * Vector3.forward, Mathf.Infinity, visualLayer);
    }
    #endregion

    private void SetGroundLevel(){
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, walkableLayer);
        groundLevel = hit.transform.position.y;
    }
}
