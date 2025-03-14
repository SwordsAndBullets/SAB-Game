using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingEntity : MonoBehaviour
{
    public float health { get; private set; }
    public float speed { get; private set; }
    public float strength {get; private set;}
    public float distance {get; private set;}

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
            Debug.Log("Took " + amount + " damage!");
        }

        //Check health state
        if (health >= maxHealth){
            health = maxHealth;
        }else if (health <= 0){
            DestroySelf();
        }
    }
    public void TempTakeDamage(float amount){
        float time = 5.0f;
        if (time > 0){
            dotTimer = time;
            dotDamage = amount;
            Debug.Log("Applied DOT");
        }//Initiate DOT
        else {
            health -= amount;
            Debug.Log("Took " + amount + " damage!");
        }

        //Check health state
        if (health >= maxHealth){
            health = maxHealth;
        }else if (health <= 0){
            DestroySelf();
        }
    }
    private void DestroySelf(){
        Debug.Log("Health at 0... Destroyed.");
        //Any animations and stuff for when dying here
        gameObject.SetActive(false);
    }

    public void Start(){
        //Temp:
        health = 10.0f;
        speed = 1.0f;
        strength = 1.0f;
        distance = 0.0f;
        //End temp.
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
