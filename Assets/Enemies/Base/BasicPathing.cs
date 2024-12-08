using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPathing : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask walkableLayer;

    [Header("Patrolling")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRange;
    private Vector3 movePoint;
    private Vector3 point;
    private bool movePointSet;

    private void Update() {
        Patrolling();   
    }

    #region Patrolling
    private void Patrolling(){
        if(!movePointSet){
            SetWalkPoint();
        }

        transform.LookAt(movePoint);
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, walkableLayer);
        float groundLevel = hit.transform.position.y;
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
}
