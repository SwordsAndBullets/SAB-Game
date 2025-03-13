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
    Transform gunHitPosition;

    private float UseDelayTimer = 0.0f;

    public SingleplayerItem(float sp, float st, float di){
        speed = sp;
        strength = st;
        distance = di;
    }
    
    public void Use(Transform Origin, Entity Target){
        switch (this.type.ToLower()){
            case "health consumable": HealthConsumableUse(Target); break;
            case "pistol": PistolUse(Origin); break;
            default: Debug.Log("Generic type."); break;
        }
    }

    public void ModelSwap(GameObject Hand){
        string modelName = this.transform.gameObject.name.ToLower();
        modelName = "ItemModels/Prefabs/" + modelName;
        //Get path to resources folder.
        //Models in folder must be lower case named.
        UnityEngine.Object model;
        Debug.Log("[Item] Model path: " + modelName);
        try { 
            model = Resources.Load(modelName);
        }
        catch { 
            model = Resources.Load("ItemModels/Prefabs/default");
            //Revert to default if there is no model.
        }
        Instantiate(model, Hand.transform);
        //Load item in scene.
    }

    #region Use Functions
    private void HealthConsumableUse(Entity Target){
        UseDelayTimer = this.speed;
        if(distance > 0){ Target.TakeDamage(this.strength, this.distance); }
        else { Target.TakeDamage(this.strength); }
    }

    private void PistolUse(Transform Origin){
        if (!(UseDelayTimer > 0)) {
            RaycastHit hit;
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, ignorePlayer);
            Debug.Log("[Pistol] Hit " + hit.transform.name);
            try { hit.transform.gameObject.GetComponent<Entity>().TakeDamage(this.strength); }
            catch { Debug.Log("[Pistol] Not an entity"); }
            UseDelayTimer = 1/(speed/60); //Speed = rpm, speed/60 = frequency(Hz), 1/f = T(s)
        }
        else { Debug.Log("[Pistol] Not Ready"); }
    }
    #endregion
    
    private void Update(){
        if(UseDelayTimer > 0){ UseDelayTimer -= Time.deltaTime; }
    }
}