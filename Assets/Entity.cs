using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//DEBUG.LOGs for testing, remove when tested.

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    public float health { get; private set; }
    
    [Header("Core Stats")]
    public float speed { get; private set; }
    public float strength {get; private set;}
    public float Distance {get; private set;}

    [Header("Data")]
    private float dotTimer = 0;
    private float dotDamage = 0;
    private float maxHealth = 0;

    public void TakeDamage(float amount, float time = 0){
        if (time > 0){
            dotTimer = time;
            dotDamage = amount;
            Debug.Log("Applied DOT");
        }//Initiate DOT
        else {
            health -= amount;
            Dbug.Log("Took " + amount + " damage!");
        }

        //Check health state
        if (health >= maxHealth){
            health = maxHealth;
        else if (health <= 0){
            DestroySelf();
        }
    }
    private void DestroySelf(){
        Debug.Log("Health at 0... Destroyed.");
        //Any animations and stuff for when dying here
        gameObject.setActive(false);
    }

    public void Start(){
        maxHealth = health;
    }
    public void Update(){
        if (dotTimer > 0){
            TakeDamage(dotDamage * Time.deltaTime);
            Debug.Log("Taking DOT: " + dotDamage + " --> " + this.health);
            dotTimer -= Time.deltaTime;
            //DOT independant of frame rate so ppl with 1000fps dont just phase out of existence.
        }
    }
}
