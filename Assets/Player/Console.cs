using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Console : MonoBehaviour
{
    [SerializeField] Player player;
    private Movement playerMovement;
    private Mouselook playerLook;
    private InputField consoleInputField;
    private bool active = false;

    public void OnSubmit(string input)
    {
        Debug.Log("Command: " + input);
    }

    void Start()
    {
        consoleInputField = gameObject.GetComponent<InputField>();
        playerMovement = player.GetComponent<Movement>();
        playerLook = player.GetComponent<Mouselook>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Console");
            active = !active;
            playerMovement.enabled = !playerMovement.enabled;
            playerLook.enabled = !playerLook.enabled;
        }
        if (active)
        {
            EventSystem.current.SetSelectedGameObject(consoleInputField.gameObject, null);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
