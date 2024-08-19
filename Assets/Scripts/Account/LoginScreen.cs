using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    [Header("References")]
    //Classes
    [SerializeField] WebFunctions webFunctions;
    [SerializeField] CursorManager cursorManager;
    //UI Elements
    [SerializeField] Text UsernameText;
    [SerializeField] Text PasswordText;
    [SerializeField] Text ErrorMessageText;
    //Game Objects
    [SerializeField] GameObject LoginObj;
    [SerializeField] Player player;
    [SerializeField] GameObject playerObj;

    private string username;
    private string password;


    public void LoginButton()
    {
        username = UsernameText.text;
        password = PasswordText.text;
        webFunctions.LoginStarter(username, password);
        cursorManager.Set("loading");
    }
    public void LoginResult(string result)
    {
        ErrorMessageText.text = result;
        cursorManager.Set();

        if (result == "Login Success!") { LoginSuccess(); }
    }
    public void LoginSuccess(){
        
        player.username = username;
        player.password = password;

        playerObj.SetActive(true);
        LoginObj.SetActive(false);
    }
    private void Start() {
        
    }
}