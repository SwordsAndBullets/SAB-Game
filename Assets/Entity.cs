using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    public float health { get; set{ ChangeHealth(value); }}
    [SerializeField] private UnityEvent Take1Damage;

    [Header("Core Stats")]
    public float speed { get; protected set; }//Movement speed etc
    public float strength {get; protected set;}//Melee multiplier
    public float Distance {get; protected set;}//Distance stat

    public IEnumerator ChangeHealth(float amount){
        while(amount > 0){
            yield return new WaitForSeconds(0.5f);
            try { take1Damage.Invoke(); }
            catch (exception e){
                switch(e){
                    case ""/*null reference exception*/: Debug.Log("No change health function applied"); break;
                    default: Debug.Log(e);
                }
            }
        }
        health = amount;
    }
}
