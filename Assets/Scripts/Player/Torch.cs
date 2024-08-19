using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject torch;

    private void Start()
    {
        torch = GameObject.Find("Torch");
        torch.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) /*&&NotInSafeZone*/)
        {
            if (torch.activeSelf) { torch.SetActive(false); }
            else { torch.SetActive(true); }
        }
    }
}
