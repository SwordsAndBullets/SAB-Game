using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : MonoBehaviour
{
    //Core Stats
    public float speed { get; private set;}
    public float strength { get; private set;}
    public float distance { get; private set;}
    public string type;

    private float UseDelayTimer = 0.0f;
    
    #region Gun testing graphics
    private bool traceGunshot = false;
    private Ray trace;
    #endregion

    public Item(float sp, float st, float di, string ty){
        speed = sp;
        strength = st;
        distance = di;
        type = ty;
    }
    
    public void Use(Transform Origin, Entity Target = null){
        switch (type){
            case "health consumable": HealthConsumableUse(Target); break;
            case "pistol": PistolUse(Origin); break;
            default: Debug.Log("Generic type."); break;
        }
    }

    #region Use Functions
    private void HealthConsumableUse(Entity Target = null){
        UseDelayTimer = this.speed;
        if(distance > 0){ Target.TakeDamage(this.strength, this.distance); }
        else { Target.TakeDamage(this.strength); }//Uses Entity's take damage function.
    }

    private void PistolUse(Transform Origin){
        if (UseDelayTimer !> 0){//Allows setting a fire-rate for pistols.
            RaycastHit hit;
            trace = new Ray(Origin.position, Vector3.forward);
            traceGunshot = true;
            Physics.Raycast(trace, out hit, this.distance);
            try{ hit.transform.GetComponent<Entity>().TakeDamage(this.strength); Debug.Log("Hit " + hit.transform.name);}
            catch{ Debug.Log("Nothing Hit"); }
            Debug.Log("Pistol shot");
            UseDelayTimer = 1/(speed/60); //Speed = rpm, speed/60 = frequency(Hz), 1/f = T(s) = delayTimer
        }
    }
    #endregion

    private void Update(){
        if(UseDelayTimer > 0){ UseDelayTimer -= Time.deltaTime; }
    }
    private void OnDrawGizmos(){
        if (traceGunshot){
            Gizmos.color = Color.red;
            Gizmos.DrawRay(trace);
        }
    }
}