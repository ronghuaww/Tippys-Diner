using System.Collections.Generic;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public string requiredFoodTag;
    public bool OrderDone = false;

    public int playerNumber = -1;

    private List<Food> foodInTrigger = new List<Food>(); // List to track food in the trigger area

    private void Start()
    {

    }

    private void Update()
    {
        // Check for food delivery
        if(!OrderDone)
        {
            foreach (Food foodItem in foodInTrigger)
            {
                if (!foodItem.IsPickedUp && foodItem.CompareTag(requiredFoodTag))
                {
                    TryDeliverFood(foodItem);
                    break;
                }
            }
        }
    }

    public void AssignFoodOrder()
    {
        string[] possibleOrders = { "Hamburger", "Soup", "HotDog" };
        int randomIndex = Random.Range(0, possibleOrders.Length);
        requiredFoodTag = possibleOrders[randomIndex];

        Debug.Log("Customer has ordered: " + requiredFoodTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}");
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Hamburger") || otherGameObject.CompareTag("HotDog") || otherGameObject.CompareTag("Soup"))
        {
            Food foodItem = otherGameObject.GetComponent<Food>();
            if (foodItem != null && !foodItem.IsPickedUp)
            {
                foodInTrigger.Add(foodItem);
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
        Debug.Log($"Exited by: {other.name}");

        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Hamburger") || otherGameObject.CompareTag("HotDog") || otherGameObject.CompareTag("Soup"))
        {
            Food foodItem = otherGameObject.GetComponent<Food>();
            if (foodItem != null)
            {
                foodInTrigger.Remove(foodItem);
                Debug.Log($"{foodItem.name} exited the trigger area.");
            }
        }
    }

    private void TryDeliverFood(Food foodItem)
    {
        playerNumber = foodItem.LastPlayerNumber;
        OrderDone = true;
        Debug.Log($"CORRECT ORDER delivered: {requiredFoodTag} by Player {playerNumber}");
    }
}
