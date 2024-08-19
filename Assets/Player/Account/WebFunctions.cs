using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class WebFunctions : MonoBehaviour
{
    #region[Login]
    public void LoginStarter(string username, string password)
    {
        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        string LoginResult = "";
        WWWForm details = new WWWForm();
        details.AddField("loginUsername", username);
        details.AddField("loginPassword", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sab/Login.php", details))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                LoginResult = www.error;
            }
            else
            {
                LoginResult = www.downloadHandler.text;
            }
        }
        Debug.Log(LoginResult);
        LoginScreen loginScreen = GameObject.Find("Login").GetComponentInChildren<LoginScreen>();
        loginScreen.LoginResult(LoginResult);
    }
    #endregion

    #region[GetPlayerInfo]
    [SerializeField] Player player;
    public void GetPlayerInfoStarter(string username, string password){
        StartCoroutine(GetPlayerInfo(username, password));
    }
    
    IEnumerator GetPlayerInfo(string username, string password){
        string result = "";
        WWWForm details = new WWWForm();
        details.AddField("username", username);
        details.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/sab/GetPlayerInfo.php", details)){
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                result = www.error;
            }else{
                result = www.downloadHandler.text;
            }
        }
        Debug.Log(result);
        //Assign Player Data
        string[] playerData = result.Split("/");
        player.username = playerData[2];
        player.password = playerData[3];
        player.id = int.Parse(playerData[1]);
        player.xp = int.Parse(playerData[5]);
    }
    #endregion
}
