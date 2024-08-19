using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    #region Enemies Remaining / Objective Fullfillment
    private List<string> enemiesList;
    private int checkObjectiveCompleteFrequency; //Frames between checking if objective for that tile is fulfilled.
    private int objectiveCheckTimer; //<-- actualCounter

    public int enemiesRemaining;
    #endregion

    private void Start()
    {
        checkObjectiveCompleteFrequency = 30; //Assign Frequency
    }

    private void Update()
    {
        switch (objectiveCheckTimer)
        {
            case 0: objectiveCheckTimer = checkObjectiveCompleteFrequency; ObjectiveCompleteCheck(); break;
            default: objectiveCheckTimer -= 1; break;
        }
    }

    #region Enemies Remaining / Objective Fullfillment
    private void ObjectiveCompleteCheck()
    {
        Transform[] enemies = transform.Find("Enemies").GetComponentsInChildren<Transform>();
        switch (enemies.Length)
        {
            case 1: Debug.Log("Objective Complete!"); ObjectiveComplete(); break;
            default: Debug.Log(enemies.Length - 1 + " Enemies Remaining"); break;
        }
        enemiesRemaining = enemies.Length - 1; //Used by external parts eg: HUD Elements.
    }

    private void ObjectiveComplete()
    {
        
    }
    #endregion
}
