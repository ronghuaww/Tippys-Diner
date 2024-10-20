using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawn : MonoBehaviour
{
    public GameObject foodPrefab;
    public Transform spawnPoint;
    public bool IsFoodGenerating { get; private set; }

    // UI Elements
    [SerializeField] private TimerBar timerBar; 
    public GameObject timerUI; 
    public float generationTime = 3f;

    private void Start()
    {
        timerUI.SetActive(false);
    }

    private void Update()
    {
        // FaceCamera();
    }

    public void GenerateFood()
    {
        if (IsFoodGenerating) return; // Prevent multiple generation if a food is being generated

        IsFoodGenerating = true;
        timerBar.ResetBar(); 
        timerUI.SetActive(true);
        StartCoroutine(FoodGenerationCountdown());
    }

    private IEnumerator FoodGenerationCountdown()
    {
        float elapsedTime = 0f;

        // Countdown to food generation
        while (elapsedTime < generationTime)
        {
            elapsedTime += Time.deltaTime; 
            timerBar.UpdateTime(elapsedTime, generationTime);
            yield return null;
        }

        SpawnFood();
        IsFoodGenerating = false;
    }

    private void SpawnFood()
    {
        // Instantiate food at the spawn point
        GameObject newFood = Instantiate(foodPrefab, spawnPoint.position, Quaternion.identity);
        newFood.GetComponent<Collider>().isTrigger = true;
        timerUI.SetActive(false);

        // Food Lifetime
        StartCoroutine(FoodLifetime(newFood));
    }

    private IEnumerator FoodLifetime(GameObject food)
    {
        float foodLifetime = 10f;
        yield return new WaitForSeconds(foodLifetime);
        if(food != null)
        {
            Destroy(food);
        }
    }

    private void FaceCamera()
    {
        if (timerUI != null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                timerUI.transform.LookAt(timerUI.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}
