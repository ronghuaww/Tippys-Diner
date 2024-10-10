using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab; // Prefab of the food to spawn
    public Transform spawnPoint; // Point where food will be spawned
    public bool IsFoodGenerating { get; private set; } // Indicates if food is being generated
    private GameObject currentFood; // Reference to the currently generated food object

    private void Start()
    {
        // GenerateFood();
    }

    public void GenerateFood()
    {
        if (IsFoodGenerating || currentFood != null) return; // Prevent multiple generation

        IsFoodGenerating = true;

        // Instantiate food at the spawn point
        currentFood = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        currentFood.GetComponent<Collider>().isTrigger = true; // Ensure the food collider is a trigger

        // Optionally, you can add more logic here, like setting a lifespan for the food
        StartCoroutine(FoodLifetime(currentFood));
    }

    private IEnumerator FoodLifetime(GameObject food)
    {
        yield return new WaitForSeconds(10f); // Duration before food is destroyed
        Destroy(food);
        currentFood = null; // Clear reference after destruction
        IsFoodGenerating = false; // Reset generation flag
    }
}
