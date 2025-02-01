using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    PlayerInput playerInput;
    Rigidbody rb;

    [SerializeField] Transform cam;


    [Header("Movement")]
    Vector3 moveVect;
    [SerializeField] private float moveForce = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerInput = new PlayerInput();
        playerInput.Movement.Enable();
        playerInput.Movement.Move.performed += Move_performed;


    }

    private void Move_performed(InputAction.CallbackContext ctx)
    {
        moveVect = ctx.ReadValue<Vector3>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //x = A/D | y = Space/LShift | z = W/S
        Vector3 direction = (cam.forward.normalized * moveVect.z) + (cam.right.normalized * moveVect.x) + (Vector3.up * moveVect.y);

        rb.AddForce(direction * moveForce, ForceMode.Acceleration);

    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
