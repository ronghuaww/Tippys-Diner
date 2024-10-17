using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Food currentFood; // Reference to the food object the player is near
    private FoodSpawn currentFoodSpawn; // Reference to the food spawn point the player is near
    public Transform carryPoint; // Reference to the carry point for food

    private PlayerController playerController; // Reference to PlayerController to get player number
    private PlayerAnimatorController playerAnimatorController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>(); // Get the PlayerController component
        playerAnimatorController = GetComponent<PlayerAnimatorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FoodSpawn"))
        {
            currentFoodSpawn = other.GetComponent<FoodSpawn>();
        }
        else if (other.CompareTag("Hamburger") || other.CompareTag("HotDog") || other.CompareTag("Soup"))
        {
            currentFood = other.GetComponent<Food>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FoodSpawn") && currentFoodSpawn != null)
        {
            currentFoodSpawn = null;
        }
        else if ((other.CompareTag("Hamburger") || other.CompareTag("HotDog") || other.CompareTag("Soup")) && currentFood != null)
        {
            currentFood = null;
        }
    }

public void OnInteract(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        // If there is food available to interact with
        if (currentFood != null)
        {
            if (!currentFood.IsPickedUp || currentFood.LastPlayerNumber == playerController.playerNumber)
            {
                // If the food is not picked up or is being held by the current player, interact with it
                Debug.Log("Interacting with food!");
                currentFood.PlayerInteract(gameObject); // Pass the player's GameObject to the method

                // Trigger the correct animation based on the interaction state
                if (currentFood.IsPickedUp)
                {
                    playerAnimatorController.TriggerPickupAnimation();
                }
                else
                {
                    playerAnimatorController.TriggerPutdownAnimation();
                }
            }
        }
        // If no food is present, interact with the food spawn to order food
        else if (currentFoodSpawn != null && !currentFoodSpawn.IsFoodGenerating)
        {
            Debug.Log("Ordering food!");
            playerAnimatorController.TriggerPutdownAnimation();
            currentFoodSpawn.GenerateFood();
        }
    }
}


    public Food GetCarriedFood()
    {
        return currentFood; // Return the currently carried food
    }

    public void DropCarriedFood()
    {
        if (currentFood != null)
        {
            currentFood.Drop(playerController.playerNumber); // Pass the player number to drop
            currentFood = null; // Clear the reference
        }
    }
}
