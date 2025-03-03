using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    [Header("----Player Info----")]
    public string username = "";
    public string password = "";
    public int id = 0;
    public int xp = 0;

    [Header("----References----")]
    [SerializeField] WebFunctions wf;
    [SerializeField] Entity playerEntity;

    private void Start() {
        if (username != "" || username != null){
            GetPlayerInfoStarter();
        }
    }

    public void GetPlayerInfoStarter(){
        StartCoroutine(GetPlayerInfo(username, password, this));
    }

    #region WebFunctions
    IEnumerator GetPlayerInfo(string username, string password, Player p){
        string result = "";
        WWWForm details = new WWWForm();
        details.AddField("username", username);
        details.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post(wf.urlStarter + "/sab/GetPlayerInfo.php", details)){
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
        switch (playerData[0]){
            case "private":
                p.id = int.Parse(playerData[1]);
                p.username = playerData[2];
                p.password = playerData[3];
                p.xp = int.Parse(playerData[5]);
                break;
            case "public":
                Debug.Log(result);
                break;
        }
    }
    #endregion
}
