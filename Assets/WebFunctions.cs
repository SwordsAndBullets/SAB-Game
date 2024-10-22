using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class WebFunctions : MonoBehaviour
{
    [Header("Data")]
    public string urlStarter = "http://localhost";

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
        using (UnityWebRequest www = UnityWebRequest.Post(urlStarter + "/sab/Login.php", details))
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
        LoginScreen loginScreen = GameObject.Find("Login").GetComponentInChildren<LoginScreen>();
        loginScreen.LoginResult(LoginResult);
    }
    #endregion
}
