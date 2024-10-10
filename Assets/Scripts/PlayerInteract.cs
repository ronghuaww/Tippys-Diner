using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Food currentFood; // Reference to the food object the player is near
    private FoodSpawn currentFoodSpawn; // Reference to the food spawn point the player is near
    public Transform carryPoint; // Reference to the carry point for food

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters a FoodSpawn trigger area
        if (other.CompareTag("FoodSpawn"))
        {
            currentFoodSpawn = other.GetComponent<FoodSpawn>();
        }
        // Check if the player enters a food object's trigger area
        else if (other.CompareTag("Hamburger")|| other.CompareTag("HotDog")||other.CompareTag("Soup"))
        {
            currentFood = other.GetComponent<Food>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits a FoodSpawn trigger area
        if (other.CompareTag("FoodSpawn") && currentFoodSpawn != null)
        {
            currentFoodSpawn = null;
        }
        // Check if the player exits a food object's trigger area
        else if ((other.CompareTag("Hamburger")|| other.CompareTag("HotDog")||other.CompareTag("Soup")) && currentFood != null)
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
            Debug.Log("Interacting with food!");
            currentFood.PlayerInteract();
        }
        // If no food is present, interact with the food spawn to order food
        else if (currentFoodSpawn != null && !currentFoodSpawn.IsFoodGenerating)
        {
            Debug.Log("Ordering food!");
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
        currentFood.Drop(); // Drop the food
        currentFood = null; // Clear the reference
    }
}


}
