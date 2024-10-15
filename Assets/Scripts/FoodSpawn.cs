using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab; // Prefab of the food to spawn
    public Transform spawnPoint; // Point where food will be spawned
    public bool IsFoodGenerating { get; private set; } // Indicates if food is being generated
    private GameObject currentFood; // Reference to the currently generated food object

    // UI Elements
    [SerializeField] private TimerBar timerBar; // Reference to the TimerBar script
    public GameObject timerUI; // Reference to the UI panel or game object that holds the timer slider
    public float generationTime = 3f; // Time before food is generated

    private void Start()
    {
        timerUI.SetActive(false); // Initially hide the timer UI
    }

    public void GenerateFood()
    {
        if (IsFoodGenerating || currentFood != null) return; // Prevent multiple generation

        IsFoodGenerating = true;
        timerBar.ResetBar(); // Reset the timer bar
        timerUI.SetActive(true); // Show the timer UI
        StartCoroutine(FoodGenerationCountdown());
    }

    private IEnumerator FoodGenerationCountdown()
    {
        float elapsedTime = 0f;

        // Countdown to food generation
        while (elapsedTime < generationTime)
        {
            elapsedTime += Time.deltaTime; // Increment elapsed time
            timerBar.UpdateTime(elapsedTime, generationTime); // Update the timer bar based on elapsed time
            yield return null; // Wait for the next frame
        }

        // After the countdown, spawn the food
        SpawnFood();
    }

    private void SpawnFood()
    {
        // Instantiate food at the spawn point
        currentFood = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        currentFood.GetComponent<Collider>().isTrigger = true; // Ensure the food collider is a trigger
        timerUI.SetActive(false); // Hide the timer UI

        // Food Lifetime
        StartCoroutine(FoodLifetime(currentFood));
    }

    private IEnumerator FoodLifetime(GameObject food)
    {
        float foodLifetime = 10f; // Duration before food is destroyed
        yield return new WaitForSeconds(foodLifetime); // Wait for food to be available
        Destroy(food); // Destroy food
        currentFood = null; // Clear reference after destruction
        IsFoodGenerating = false; // Reset generation flag
    }
}
