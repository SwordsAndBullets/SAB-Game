using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour
{
    [Header("References")]
    //Classes
    [SerializeField] WebFunctions webFunctions;
    [SerializeField] CursorManager cursorManager;
    //UI Elements
    [SerializeField] Text usernameText;
    [SerializeField] Text passwordText;
    [SerializeField] Text errorMessageText;
    //Game Objects
    [SerializeField] GameObject loginObject;
    [SerializeField] Player player;
    [SerializeField] GameObject playerObj;

    private string username;
    private string password;


    public void LoginButton()
    {
        username = usernameText.text;
        password = passwordText.text;
        webFunctions.LoginStarter(username, password);
        cursorManager.Set("loading");
    }
    public void LoginResult(string result)
    {
        errorMessageText.text = result;
        cursorManager.Set();

        if (result == "Login Success!") { LoginSuccess(); }
    }
    private void LoginSuccess(){
        
        player.name = username;
        player.login = password;

        playerObj.SetActive(true);
        loginObject.SetActive(false);
    }
}