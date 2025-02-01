using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[Header("References")]
    PlayerInput playerInput;
    Rigidbody rb;
    Transform cam;


    [Header("Movement")]
    Vector3 moveVect;
    [SerializeField] private float moveForce = 1f;
    [SerializeField] private float boostForce = 1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerInput = new PlayerInput();
        playerInput.Movement.Enable();
        playerInput.Movement.Move.performed += Move_performed;
        playerInput.Movement.Boost.performed += Boost_performed;

        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Boost_performed(InputAction.CallbackContext ctx)
    {
        rb.AddForce(cam.forward.normalized * boostForce, ForceMode.Impulse);
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
        //move direction taking into account the direction camera is facing
        Vector3 dir = (cam.forward.normalized * moveVect.z) + (cam.right.normalized * moveVect.x) + (Vector3.up * moveVect.y);
        rb.AddForce(dir * moveForce, ForceMode.Force);

        //rotate player with camera (not sure if this is even needed?
        transform.rotation = cam.rotation;

    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
