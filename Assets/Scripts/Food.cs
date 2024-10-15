using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour, IInteractable
{
    private bool isPickedUp = false; // To track if the food is already picked up
    private int lastPlayerNumber = -1; // To keep track of the last player number who held the food

    public bool IsPickedUp => isPickedUp; // Public property to access the picked-up state
    public int LastPlayerNumber => lastPlayerNumber; // Public property to access the last player number who held the food

    private Rigidbody rb; // Rigidbody component

    private void Awake()
    {
        // Ensure the food has a Rigidbody component for physics interactions
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Initialize Rigidbody properties
        rb.useGravity = false; // Initially, gravity is off
        rb.isKinematic = true; // Initially, the food is kinematic
    }

    public void PlayerInteract(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Transform carryPoint = playerController.carryPoint; // Get the carry point from the player
            int playerNumber = playerController.playerNumber; // Get the player number from the PlayerController

            // Check if the food is already picked up by another player
            if (!isPickedUp || lastPlayerNumber == playerNumber)
            {
                // Allow the player to interact with the food only if it is not picked up 
                // or if the player is the same player who last held it
                if (!isPickedUp)
                {
                    PickUp(carryPoint, playerNumber);
                }
                else
                {
                    Drop(playerNumber);
                }
            }
            else
            {
                Debug.Log("Food is already picked up by another player!");
            }
        }
    }

    private void PickUp(Transform carryPoint, int playerNumber)
    {
        if (!isPickedUp)
        {
            // Set the food's Rigidbody to kinematic while picked up
            rb.isKinematic = true;
            rb.useGravity = false; // Disable gravity when picked up

            // Set the position of the food to the carry point
            transform.position = carryPoint.position;
            transform.SetParent(carryPoint); // Set the carry point as the parent
            isPickedUp = true;

            // Disable the collider to prevent further interactions
            Collider foodCollider = GetComponent<Collider>();
            if (foodCollider != null)
            {
                foodCollider.enabled = false;
            }

            lastPlayerNumber = playerNumber; // Update the last player number who picked up the food
            Debug.Log("Food picked up by player number: " + playerNumber);
        }
        else
        {
            Debug.Log("Food is already picked up!");
        }
    }

    public void Drop(int playerNumber)
    {
        if (isPickedUp)
        {
            Transform playerTransform = FindPlayerTransformByNumber(playerNumber);
            if (playerTransform != null)
            {
                // Detach from the carry point
                transform.SetParent(null); 

                // Apply a slight forward impulse
                rb.isKinematic = false; // Set Rigidbody back to non-kinematic
                rb.useGravity = true; // Enable gravity when dropped

                // Calculate the forward direction relative to the player and apply a force
                Vector3 forwardDirection = playerTransform.forward;
                rb.AddForce(forwardDirection * 5f, ForceMode.Impulse); // Adjust the force value as needed

                // Re-enable the collider after being dropped
                Collider foodCollider = GetComponent<Collider>();
                if (foodCollider != null)
                {
                    foodCollider.enabled = true;
                }

                isPickedUp = false; // Update the state to not picked up
                lastPlayerNumber = playerNumber; // Keep track of who dropped the food
                Debug.Log("Food dropped by player number: " + playerNumber);
            }
            else
            {
                Debug.Log("Player transform not found!");
            }
        }
        else
        {
            Debug.Log("Food is not currently being carried!");
        }
    }

    private Transform FindPlayerTransformByNumber(int playerNumber)
    {
        // This function searches for the player's transform using their player number
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            if (player.playerNumber == playerNumber)
            {
                return player.transform; // Return the player's transform
            }
        }
        return null; // Return null if the player is not found
    }
}
