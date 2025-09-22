using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody playerRB;
    private Entity player;

    public LayerMask groundLayer;

    private Vector3 moveInput;
    private Vector3 previousMovement;

    public float baseSpeed;
    public float baseJump;

    private float movementSpeed;
    private float jumpHeight;
    private float jumpChargesBase;
    private float jumpCharges;

    private bool isGrounded;
    private float groundCheckDistance;
    private bool isJumping;

    #region[UnityFunctions]
    private void Start()
    {
        playerRB = GetComponentInChildren<Rigidbody>();
        player = GetComponentInParent<Entity>();

        Cursor.lockState = CursorLockMode.Locked;

        groundCheckDistance = 0;
    }
    private void Update()
    {
        ResetDefaults();

        //Status updates
        if (isGrounded) { jumpCharges = jumpChargesBase; }
        GroundCheck();

        //Inputs
        if (Input.GetButtonDown("Jump")) { Jump(); }
        if (isGrounded || isJumping) { moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); }

        //Using items
        if (Input.GetButtonDown("Primary Fire")) { player.equippedItem.Use(player); }
        if (Input.GetButtonDown("Secondary Fire")) { player.secondaryItem.Use(player); }
    }
    private void FixedUpdate()
    {
        Move(moveInput);
    }
    #endregion

    #region[Tools]
    private void GroundCheck()
    {
        if (Physics.CheckSphere(transform.position - new Vector3(0, groundCheckDistance, 0), 0.1f, groundLayer)) { isGrounded = true; }
        else { isGrounded = false; }
    }
    private void ResetDefaults()
    {
        //SetValues Based On Agility
        jumpChargesBase = 1 + (player.speed / 50);
        movementSpeed = baseSpeed + ((baseSpeed / 2) * (player.speed / 100));
        jumpHeight = baseJump;
    }
    #endregion

    #region[Movement]
    private void Move(Vector3 movement)
    {
        movement = movement.normalized; //Stop Doom skate
        movement = playerRB.rotation * movement; //Move in facing direction
        movement = movement * movementSpeed * Time.fixedDeltaTime;

        if (isGrounded || isJumping) 
        {
            previousMovement = movement;
            playerRB.MovePosition(playerRB.position + movement);
            isJumping = false;
        }
        else { playerRB.MovePosition(playerRB.position + previousMovement); }
    }
    private void Jump()
    {
        if (jumpCharges > 0)
        {
            //Reset vertical velocity
            if (playerRB.velocity.y < 0) { playerRB.velocity = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z); }

            if (!isGrounded)
            {
                //Jump (in air)
                playerRB.AddRelativeForce(0, jumpHeight, 0);
                isJumping = true;
                jumpCharges--;
            }
            else
            {
                //Jump (on ground)
                playerRB.AddRelativeForce(0, jumpHeight / 1.5f, 0);
                isJumping = true;
            }
        }
    }
    #endregion
}
