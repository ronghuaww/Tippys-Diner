using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float friction = 5f;
    private Rigidbody rb;

    public InputActionAsset inputActionAsset; // Reference to the Input Action Asset
    public Transform carryPoint; // Transform where the food will be attached when picked up

    public int playerNumber; // 1 or 2 to determine the player

    private InputAction moveAction; // Reference to the move action
    private InputAction interactAction; // Reference to the interact action

    private Vector2 moveInput;

    private PlayerInteract playerInteract; // Reference to the PlayerInteract script

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = friction;

        AssignActionsBasedOnPlayerNumber();

        // Get the PlayerInteract component on the same GameObject
        playerInteract = GetComponent<PlayerInteract>();
    }

    void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;
        }

        if (interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteract; // Link the interact action
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
            moveAction.Disable();
        }

        if (interactAction != null)
        {
            interactAction.performed -= OnInteract;
            interactAction.Disable();
        }
    }

    void Update()
    {
        Move();
    }

void Move()
{
    Vector3 forward = Camera.main.transform.forward;
    Vector3 right = Camera.main.transform.right;

    forward.y = 0f; 
    right.y = 0f;

    forward.Normalize();
    right.Normalize();

    Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

    rb.AddForce(moveDirection * moveSpeed);

    // Rotate the player to face the movement
    if (moveDirection != Vector3.zero)
    {
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    // Clamp the velocity to the maximum speed
    if (rb.velocity.magnitude > maxSpeed)
    {
        rb.velocity = rb.velocity.normalized * maxSpeed;
    }

    // Slow down the player smoothly when no input is provided
    if (moveInput == Vector2.zero)
    {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, friction * Time.deltaTime);
    }
}


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveInput = Vector2.zero; 
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && playerInteract != null)
        {
            playerInteract.OnInteract(context); // Call the OnInteract method in PlayerInteract
        }
    }

    private void AssignActionsBasedOnPlayerNumber()
    {
        // Get the action map based on the player number
        var actionMapName = playerNumber == 1 ? "Player1" : "Player2";
        var actionMap = inputActionAsset.FindActionMap(actionMapName);

        if (actionMap != null)
        {
            // Retrieve the move and interact actions from the action map
            moveAction = actionMap.FindAction("Move");
            interactAction = actionMap.FindAction("Interact");

            if (moveAction == null || interactAction == null)
            {
                Debug.LogError($"Actions not found in the action map '{actionMapName}'.");
            }
        }
        else
        {
            Debug.LogError($"Action map '{actionMapName}' not found in the Input Action Asset.");
        }
    }
}
