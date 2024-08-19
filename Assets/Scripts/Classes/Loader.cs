using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    private bool loaded;

    public bool loggedIn;
    public bool playerData_Ready;

    private void FixedUpdate() {
        if (!loaded){
            if (loggedIn && playerData_Ready){
                Debug.Log("loading complete");
                loaded = true;
            }
        }
    }
}