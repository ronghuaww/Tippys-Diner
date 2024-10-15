using System.Collections.Generic;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public string requiredFoodTag;
    public bool OrderDone = false;

    private List<Food> foodInTrigger = new List<Food>(); // List to track food in the trigger area

    private void Start()
    {
        AssignFoodOrder();
    }

    private void Update()
    {
        // Check for food delivery
        foreach (Food foodItem in foodInTrigger)
        {
            if (!foodItem.IsPickedUp && foodItem.CompareTag(requiredFoodTag))
            {
                TryDeliverFood(foodItem);
                break; // Stop checking after the first correct order is delivered
            }
        }
    }

    // Method to assign a random food order to the customer
    private void AssignFoodOrder()
    {
        string[] possibleOrders = { "Hamburger", "Soup", "HotDog" };
        int randomIndex = Random.Range(0, possibleOrders.Length);
        requiredFoodTag = possibleOrders[randomIndex];

        Debug.Log("Customer has ordered: " + requiredFoodTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}"); // Log which object triggered the event

        // Check if the collider's gameObject is food
        GameObject otherGameObject = other.gameObject; // Get the GameObject from the collider
        if (otherGameObject.CompareTag("Hamburger") || otherGameObject.CompareTag("HotDog") || otherGameObject.CompareTag("Soup"))
        {
            Food foodItem = otherGameObject.GetComponent<Food>();
            if (foodItem != null && !foodItem.IsPickedUp) // Ensure food is not picked up
            {
                foodInTrigger.Add(foodItem); // Add food item to the list
                Debug.Log($"{foodItem.name} entered the trigger area.");
            }
            else
            {
                Debug.Log($"{otherGameObject.name} is either not a Food object or is already picked up.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exited by: {other.name}"); // Log which object exited the event

        // Check if the collider's gameObject is food
        GameObject otherGameObject = other.gameObject; // Get the GameObject from the collider
        if (otherGameObject.CompareTag("Hamburger") || otherGameObject.CompareTag("HotDog") || otherGameObject.CompareTag("Soup"))
        {
            Food foodItem = otherGameObject.GetComponent<Food>();
            if (foodItem != null)
            {
                foodInTrigger.Remove(foodItem); // Remove food item from the list
                Debug.Log($"{foodItem.name} exited the trigger area.");
            }
        }
    }

    private void TryDeliverFood(Food foodItem)
    {
        // Check if the food item matches the required food tag
        int playerNumber = foodItem.LastPlayerNumber;
        OrderDone = true; // Mark the order as completed
        Debug.Log($"CORRECT ORDER delivered: {requiredFoodTag} by Player {playerNumber}");
        
        // Optionally, remove the food item from the trigger list
        foodInTrigger.Remove(foodItem);
    }
}
