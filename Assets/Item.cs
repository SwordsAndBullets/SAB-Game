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
    public string type {get; private set;}

    private float UseDelayTimer = 0.0f;

    public Item(float sp, float st, float di){
        speed = sp;
        strength = st;
        distance = di;
    }
    
    public void Use(Transform Origin, Entity Target = null){
        switch (this.type){
            case "Health Consumable": HealthConsumableUse(Target); break;
            case "Pistol": PistolUse(Origin); break;
            default: Debug.Log("Generic type."); break;
        }
    }

    #region Use Functions
    private void HealthConsumableUse(Entity Target = null){
        UseDelayTimer = this.speed;
        if(distance > 0){ Target.TakeDamage(this.strength, this.distance); }
        else { Target.TakeDamage(this.strength); }
    }

    private void PistolUse(Transform Origin){
        if (UseDelayTimer !> 0){
            RaycastHit hit;
            Physics.Raycast(Origin.position, Vector3.forward, out hit, this.distance);
            try{ hit.transform.GetComponent<Entity>().TakeDamage(this.strength); }
            catch{ Debug.Log("Nothing Hit"); }
            Debug.Log("Pistol shot");
            UseDelayTimer = 1/(speed/60); //Speed = rpm, speed/60 = frequency(Hz), 1/f = T(s)
        }
    }
    #endregion

    private void Update(){
        if(UseDelayTimer > 0){ UseDelayTimer -= Time.deltaTime; }
    }
}