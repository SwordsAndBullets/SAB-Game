using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region[data]
    [Header("----Stats----")]
    public int level;
    public int health;

    [Header("----Abilities----")]
    public int ability1Charge;//On player :: Grenade
    public int ability2Charge;//On player :: Melee

    [Header("----Ability Recharges----")]
    public int strength; //melee cooldown
    public int agility; //speed + reload speed
    public int armament; //grenade cooldown

    [Header("----Keywords----")]
    public bool burning;
    public bool weakened;
    public bool frozen;
    #endregion

    [Header("----References----")]
    private Item[] armourItems;
    private Item[] weaponItems;
}
