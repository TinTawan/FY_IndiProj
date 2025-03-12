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


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += Move_performed;
        playerInput.Player.Move.canceled += Move_cancelled;
        playerInput.Player.VerticalMove.performed += VerticalMove_performed;
        playerInput.Player.VerticalMove.canceled += VerticalMove_cancelled;
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
        moveVect = ctx.ReadValue<Vector2>();
    }
    private void Move_cancelled(InputAction.CallbackContext ctx)
    {
        moveVect = Vector2.zero;
    }
    private void VerticalMove_performed(InputAction.CallbackContext ctx)
    {
        vertMove = ctx.ReadValue<float>();
    }
    private void VerticalMove_cancelled(InputAction.CallbackContext ctx)
    {
        vertMove = 0;
    }
    private void Boost_performed(InputAction.CallbackContext ctx)
    {
        if (canBoost)
        {
            canBoost = false;
            rb.AddForce(cam.forward.normalized * boostForce, ForceMode.Impulse);
            boostTimer = boostCD;
        }
    }

    private void Update()
    {
        BoostCooldown();
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
}
