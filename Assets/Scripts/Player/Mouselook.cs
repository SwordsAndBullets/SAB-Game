using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouselook : MonoBehaviour
{
    private Transform Head;
    private Transform Body;

    public float sensitivity = 400f;

    private float mouseX = 0;
    private float mouseY = 0;

    private void Start()
    {
        Body = transform;
        Head = Body.GetChild(0).GetChild(0) as Transform;
    }

    private void Update()
    {
        mouseX += Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        mouseY -= Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -90, 90);

        Head.transform.localRotation = Quaternion.Euler(mouseY, 0, 0);
        Body.transform.localRotation = Quaternion.Euler(0, mouseX, 0);
    }
}
