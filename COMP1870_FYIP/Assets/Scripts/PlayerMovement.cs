using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    Rigidbody rb;

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
        //Debug.Log(moveVect);
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        //x = A/D | y = Space/LShift | z = W/S
        //moveVect = moveAction.ReadValue<Vector3>();
        //Vector3 direction = (transform.forward * moveVect.x) + (transform.right * moveVect.z) + (Vector3.up * moveVect.y);

        rb.AddForce(moveVect * moveForce, ForceMode.Acceleration);

    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
