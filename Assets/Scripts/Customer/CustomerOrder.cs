using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerOrder : MonoBehaviour
{
    public Transform EatingPoint { get; set; }
    public string requiredFoodTag;


    public bool OrderDone = false;

    public int playerNumber = -1;

    public Image hamburgerIcon; 
    public Image hotdogIcon; 
    public Image soupIcon; 


    private List<Food> foodInTrigger = new List<Food>(); // List to track food in the trigger area



    // Reference to the eating point where the food should snap to
    public Transform eatingPoint;

    void Awake()
    {
        foreach(var icon in GetComponentsInChildren<Image>(true))
        {
            if(icon.name == "HamburgerIcon") 
            {
                hamburgerIcon = icon;

            } else if (icon.name == "HotdogIcon") 
            {
                hotdogIcon = icon;

            } else if (icon.name == "SoupIcon") 
            {
                soupIcon = icon;
            }
        }
    }

    private void Start()
    {
        hamburgerIcon.enabled = false;
        hotdogIcon.enabled = false;
        soupIcon.enabled = false;
    }

    private void Update()
    {
        // Check for food delivery
        if (!OrderDone)
        {
            // Remove any null food items from the list
            foodInTrigger.RemoveAll(item => item == null);

            foreach (Food foodItem in foodInTrigger)
            {
                // Check if food item is valid and has not been picked up
                if (foodItem != null && !foodItem.IsPickedUp && foodItem.CompareTag(requiredFoodTag))
                {
                    TryDeliverFood(foodItem);
                    break; // Exit loop after finding the first matching food
                }
            }
        }
    }

    public void AssignFoodOrder()
    {
        string[] possibleOrders = { "Hamburger", "Soup", "HotDog" };
        int randomIndex = Random.Range(0, possibleOrders.Length);
        requiredFoodTag = possibleOrders[randomIndex];

        if (randomIndex == 0) {
        hamburgerIcon.enabled = true;

        } else if (randomIndex == 1) {
            soupIcon.enabled = true;
            
        } else if (randomIndex == 2) {
            hotdogIcon.enabled = true;
        }


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
        playerNumber = foodItem.LastPlayerNumber; // Get the player number who picked the food
        OrderDone = true; // Mark the order as done
        Debug.Log($"CORRECT ORDER delivered: {requiredFoodTag} by Player {playerNumber}");

        // Parent the food to the customer
        ParentFoodToCustomer(foodItem);
    }

    private void ParentFoodToCustomer(Food foodItem)
    {
        // Ensure eatingPoint is assigned
        if (eatingPoint != null)
        {
            // Parent the food object to the customer
            foodItem.transform.SetParent(eatingPoint); // Parent to the eating point
            foodItem.transform.localPosition = Vector3.zero; // Snap food to the eating point
        }
        else
        {
            Debug.LogWarning("Eating point not assigned on " + gameObject.name);
        }
    }
}
