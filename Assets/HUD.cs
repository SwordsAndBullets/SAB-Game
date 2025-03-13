using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text healthText;

    [Header("External")]
    [SerializeField] private Entity player;

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.health;
    }
}
