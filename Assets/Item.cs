using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : MonoBehaviour
{
    public bool equipped = false;

    #region[Data]
    [Header("Info")]
    public int id;
    public string name;
    public string description;
    public string type;
    public string category;
    public int rarity;
    public int level;

    [Header("Stats")]
    public int strength;
    public int agility;
    public int power;
    public int armament;

    [Header("Interaction")]
    public string extra;
    public bool modify;
    #endregion

    #region[WeaponFunctions]
    private float rpm;
    private float burst;
    private float burstGap;

    private float weaponRpmTimer;
    private int burstCounter;
    private float burstGapTimer;
    private void Shoot()
    {
        if (burst == 0)
        {
            if (weaponRpmTimer <= 0)
            {
                Debug.Log("Shoot");
                weaponRpmTimer = 1 / (rpm / 60);
            }
        }
        else if (burstGapTimer <=0)
        {
            burstCounter = Mathf.RoundToInt(burst);
            while (burstCounter > 0)
            {
                if (weaponRpmTimer <= 0)
                {
                    Debug.Log("Shoot");
                    weaponRpmTimer = 1 / (rpm / 60);
                    burstCounter--;
                }
            }
            burstGapTimer = burstGap;
        }
    }//Burst crashes currently
    private void ADS()
    {
        Debug.Log("ADS");
    }
    private void Reload()
    {
        Debug.Log("Reload");
    }
    #endregion
    #region[ConsumableFunctions]
    private void Consume()
    {
        Debug.Log("Consume");
    }
    #endregion

    private void Update()
    {
        if (equipped)
        {
            switch (category)
            {
                case "weapon":
                    if (Input.GetKey(KeyCode.Mouse0)) { Shoot(); }
                    if (Input.GetKey(KeyCode.Mouse1)) { ADS(); }
                    if (Input.GetKey(KeyCode.R)) { Reload(); }
                    break;
                case "consumable":
                    if (Input.GetAxis("Primary Fire") > 0) { Consume(); }
                    break;
            }
        }

        if (weaponRpmTimer > 0) { weaponRpmTimer -= Time.deltaTime; }
        if (burstGapTimer > 0) { burstGapTimer -= Time.deltaTime; }
    }
    private void Start()
    {
        ResyncValues();
    }

    public void ResyncValues()
    {
        switch (category)
        {
            case "weapon":
                switch (extra)
                {
                    case "precision auto rifle": rpm = 400; burst = 0; burstGap = 0; break;
                    case "agressive auto rifle": rpm = 900; burst = 0; burstGap = 0; break;
                    case "precision pulse rifle": rpm = 600; burst = 2; burstGap = 0.2f; break;
                    case "agressive pulse rifle": rpm = 800; burst = 3; burstGap = 0.4f; break;
                }// Define RPM for each archetype
                break;
        }
        
    }
}
