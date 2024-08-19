using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("----Player Info----")]
    public string username = "";
    public string password = "";
    public int id = 0;
    public int xp = 0;

    [Header("----References----")]
    [SerializeField] WebFunctions webFunctions;

    private void Start() {
        if (username != "" || username != null){
            webFunctions.GetPlayerInfoStarter(username, password);
        }
    }
}
