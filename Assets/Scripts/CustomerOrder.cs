using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    public string requiredFoodTag; // The tag of the food the customer requires
    public float maxWaitTime = 60f; // Maximum time the customer will wait for their food
    public float satisfactionTip = 5f; // Tip amount if the customer is satisfied
    public float dissatisfactionPenalty = 0f; // Penalty if the customer leaves unhappy

    private float waitTimer; // Timer to track how long the customer has been waiting
    private bool isSatisfied = false; // Whether the customer is satisfied with their order

    private void Start()
    {
        // Initialize the wait timer and set the food order
        waitTimer = maxWaitTime;
        AssignFoodOrder();
    }

    private void Update()
    {
        if (!isSatisfied)
        {
            waitTimer -= Time.deltaTime; // Decrease the wait time

            if (waitTimer <= 0f)
            {
                HandleUnhappyCustomer(); // Customer leaves unhappy when the timer runs out
            }
        }
    }

    // Method to assign a random food order to the customer
    private void AssignFoodOrder()
    {
        string[] possibleOrders = { "Hamburger", "Soup", "HotDog"};
        int randomIndex = Random.Range(0, possibleOrders.Length);
        requiredFoodTag = possibleOrders[randomIndex];

        Debug.Log("Customer has ordered: " + requiredFoodTag);
    }

    // Method called when an object enters the customer's trigger area
private void OnTriggerEnter(Collider other)
{
    // Check if the player enters the trigger area with the food
    if (other.CompareTag("Player"))
    {
        PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
        PlayerController playerController = other.GetComponent<PlayerController>(); // Get the PlayerController

        if (playerInteract != null && playerController != null)
        {
            int playerNumber = playerController.playerNumber; // Get the player number from PlayerController
            TryDeliverFood(playerInteract, playerNumber); // Pass the player number
        }
    }
}


    // Method to check if the player has delivered the correct food
private void TryDeliverFood(PlayerInteract playerInteract, int playerNumber)
{
    Food carriedFood = playerInteract.GetCarriedFood(); // Retrieve the food the player is carrying

    // Check if the carried food is not null, has the correct tag, and is not picked up
    if (carriedFood != null && carriedFood.CompareTag(requiredFoodTag) && !carriedFood.IsPickedUp)
    {
        HandleSatisfiedCustomer(); // Handle the satisfied customer if the correct food is delivered
        playerInteract.DropCarriedFood(); // Drop the food after delivering it

        // Update the income based on the player number
        IncomeManager incomeManager = FindObjectOfType<IncomeManager>();
        if (incomeManager != null)
        {
            float incomeAmount = 10f; // Set the income amount based on the food delivered
            incomeManager.AddIncome(playerNumber, incomeAmount); // Pass the player number
        }
    }
    else if (carriedFood != null && !carriedFood.CompareTag(requiredFoodTag))
    {
        Debug.Log("Wrong food item!"); // Feedback for the wrong food
    }
    else
    {
        Debug.Log("No food available to deliver!"); // Feedback if no food is carried
    }
}


    // Handle customer satisfaction
    private void HandleSatisfiedCustomer()
    {
        isSatisfied = true;
        Debug.Log("Customer is satisfied and will leave a tip!");

        // Add the tip to the player's score, handle animations, etc.
        LeaveRestaurant(); // Make the customer leave the restaurant
    }

    // Handle customer dissatisfaction
    private void HandleUnhappyCustomer()
    {
        Debug.Log("Customer is unhappy and will leave without tipping!");
        LeaveRestaurant(); // Make the customer leave the restaurant
    }

    // Method to handle customer leaving the restaurant
    private void LeaveRestaurant()
    {
        // Code to make the customer leave the restaurant, e.g., move to an exit point
        Destroy(gameObject, 2f); // Destroy the customer object after a short delay
    }
}
