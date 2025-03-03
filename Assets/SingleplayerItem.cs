using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SingleplayerItem : MonoBehaviour
{
    //Core Stats
    public float speed;
    public float strength;
    public float distance;
    public string type;

    [SerializeField] private LayerMask ignorePlayer;

    private float UseDelayTimer = 0.0f;

    public SingleplayerItem(float sp, float st, float di){
        speed = sp;
        strength = st;
        distance = di;
    }
    
    public void Use(Transform Origin, Entity Target = null){
        switch (this.type.ToLower()){
            case "health Consumable": HealthConsumableUse(Target); break;
            case "pistol": PistolUse(Origin); break;
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
        if (!(UseDelayTimer > 0)) {
            RaycastHit hit;
            Physics.Raycast(Origin.position, Vectro3.Forward, out hit, this.distance, ignorePlayer);
            try{ hit.transform.GetComponent<Entity>().TakeDamage(this.strength); Debug.Log("[Pistol] " + hit.transform.name + " hit"); }
            catch{ Debug.Log("[Pistol] Nothing Hit"); }
            UseDelayTimer = 1/(speed/60); //Speed = rpm, speed/60 = frequency(Hz), 1/f = T(s)
        }
        else { Debug.Log("[Pistol] Not Ready"); }
    }
    #endregion
    
    private void Update(){
        if(UseDelayTimer > 0){ UseDelayTimer -= Time.deltaTime; }
    }
}