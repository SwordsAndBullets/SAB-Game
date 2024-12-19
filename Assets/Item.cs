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

    public Item(float sp, float st, float di){
        speed = sp;
        strength = st;
        distance = di;
    }
    
    public void Use(Entity Target){

    }
}
