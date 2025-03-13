using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;

    [Header("Items")]
    public SingleplayerItem EquippedItem;
    public SingleplayerItem SecondaryItem;
    [SerializeField] private GameObject PrimaryItemHand;
    [SerializeField] private GameObject SecondaryItemHand;

    [Header("Stats")]
    public float health = 10.0f;
    public float speed { get; private set; }
    public float strength {get; private set;}
    public float distance {get; private set;}

    private float dotTimer = 0;
    private float dotDamage = 0;
    private float maxHealth = 0;
    
    public void SetStats(int sp, int st, int di){
        speed = sp;
        strength = st;
        distance = di;
    }

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
    private void DestroySelf(){
        Debug.Log("Health at 0... Destroyed.");
        //Any animations and stuff for when dying here
        gameObject.SetActive(false);
    }
    private void UseEquippedItem(bool secondary = false){
        Collider temporaryPlayer = Physics.OverlapSphere(transform.position, EquippedItem.distance)[0];
        Debug.Log("[Enemy] hit " + temporaryPlayer.name);
        Entity tempPlayer = temporaryPlayer.transform.gameObject.GetComponent<Entity>();
        transform.LookAt(tempPlayer.transform.position);//Aim at player
        if(secondary){ EquippedItem.Use(this.transform, tempPlayer); }
        else { EquippedItem.Use(this.transform, tempPlayer); }//Target self as player for now as only consumables implemented with targets.
    }

    public void Start(){
        maxHealth = health;
        EquippedItem.ModelSwap(PrimaryItemHand);
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
