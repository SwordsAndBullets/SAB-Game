using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    public float health { get; set{ ChangeHealth(value); }}
    [SerializeField] private 

    [Header("Core Stats")]
    public float speed { get; protected set; }//Movement speed etc
    public float strength {get; protected set;}//Melee multiplier
    public float Distance {get; protected set;}//Distance stat

    public void ChangeHealth(float amount){
        Debug.Log("Taking " + amount + " damage!");
        health = amount;
        Debug.Log("Health: " + this.health);
    }
}
