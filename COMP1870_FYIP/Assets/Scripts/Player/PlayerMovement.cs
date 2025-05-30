using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //[Header("References")]
    PlayerInput playerInput;
    Rigidbody rb;
    Transform cam;


    [Header("Movement")]
    Vector2 moveVect;
    float vertMove;
    [SerializeField] private float moveForce = 1f;
    [SerializeField] private float boostForce = 1f, boostCD = 4f;

    bool canBoost = true;
    float boostTimer;
    public bool hurt { get; set; }
    public bool inArea { get; set; }

    bool upHeld = false, downHeld = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //playerInput = new PlayerInput();
        playerInput = InputManager.Instance.playerInput;
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += Move_performed;
        playerInput.Player.Move.canceled += Move_cancelled;
        playerInput.Player.SwimUp.performed += VerticalMoveUp_performed;
        playerInput.Player.SwimUp.canceled += VerticalMoveUp_cancelled;
        playerInput.Player.SwimDown.performed += VerticalMoveDown_performed;
        playerInput.Player.SwimDown.canceled += VerticalMoveDown_cancelled;
        playerInput.Player.Boost.performed += Boost_performed;
        playerInput.Player.Cheat.performed += Cheat_performed;

        cam = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Cheat_performed(InputAction.CallbackContext ctx)
    {
        GameManager.instance.IncrementObjects();
    }

    private void Move_performed(InputAction.CallbackContext ctx)
    {
        if (hurt)
        {
            moveVect = ctx.ReadValue<Vector2>()/4;
        }
        else
        {
            moveVect = ctx.ReadValue<Vector2>();
        }
    }
    private void Move_cancelled(InputAction.CallbackContext ctx)
    {
        moveVect = Vector2.zero;
    }
    private void VerticalMoveUp_performed(InputAction.CallbackContext ctx)
    {
        upHeld = true;
        //vertMove = ctx.ReadValue<float>();
    }
    private void VerticalMoveUp_cancelled(InputAction.CallbackContext ctx)
    {
        upHeld = false;
    }
    private void VerticalMoveDown_performed(InputAction.CallbackContext ctx)
    {
        downHeld = true;
        //vertMove = ctx.ReadValue<float>();
    }
    private void VerticalMoveDown_cancelled(InputAction.CallbackContext ctx)
    {
        downHeld = false;
    }
    private void Boost_performed(InputAction.CallbackContext ctx)
    {
        if (canBoost && !hurt)
        {
            canBoost = false;
            rb.AddForce(cam.forward.normalized * boostForce, ForceMode.Impulse);
            boostTimer = boostCD;
        }
    }

    private void Update()
    {
        BoostCooldown();

        if ((upHeld && downHeld) || (!upHeld && !downHeld))
        {
            vertMove = 0;
        }
        else if (upHeld)
        {
            vertMove = 1;
        }
        else if (downHeld)
        {
            vertMove = -1;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    void BoostCooldown()
    {
        if (boostTimer <= 0)
        {
            canBoost = true;
        }
        else
        {
            boostTimer -= Time.deltaTime;
            canBoost = false;
        }
    }

    void Move()
    {
        //move direction taking into account the direction camera is facing
        Vector3 dir = (cam.forward.normalized * moveVect.y) + (cam.right.normalized * moveVect.x) + (Vector3.up * vertMove);
        rb.AddForce(dir * moveForce, ForceMode.Force);

        //rotate player with camera (not sure if this is even needed?)
        transform.rotation = cam.rotation;

    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("HurtArea"))
        {
            inArea = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("HurtArea"))
        {
            inArea = false;
        }
    }
}
