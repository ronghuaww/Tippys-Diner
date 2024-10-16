using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    // Singleton instance
    public static IncomeManager Instance { get; private set; }

    private Dictionary<int, float> playerIncomes = new Dictionary<int, float>();
    private float player1BaseTip = 5f; // Base tip amount for Player 1
    private float player2Salary = 10f; // Fixed salary amount for Player 2

    // Make sure the inspector can show the incomes for player 1 and player 2
    [SerializeField] private float player1Income;
    [SerializeField] private float player2Income;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance if one already exists
        }
    }

    private void Start()
    {
        // Initialize incomes for two players
        playerIncomes[1] = 0f;
        playerIncomes[2] = 0f;
    }

    // Method for adding a tip to Player 1's income
    public void AddTip(int playerNumber, float happinessLevel)
    {
        if (playerNumber == 1)
        {
            // Calculate tip amount based on customer's happiness level and a random factor
            float tipAmount = player1BaseTip * Random.Range(0.5f, 1.5f) * happinessLevel;
            playerIncomes[playerNumber] += tipAmount;

            UpdatePlayerIncomeProperties();
        }
        else
        {
            Debug.LogWarning("AddTip method is only applicable to Player 1!");
        }
    }

    // Method for adding salary to Player 2's income
    public void AddSalary(int playerNumber)
    {
        if (playerNumber == 2)
        {
            playerIncomes[playerNumber] += player2Salary;
            UpdatePlayerIncomeProperties();
        }
        else
        {
            Debug.LogWarning("AddSalary method is only applicable to Player 2!");
        }
    }

    private void UpdatePlayerIncomeProperties()
    {
        // Update serialized fields for inspection
        player1Income = playerIncomes[1];
        player2Income = playerIncomes[2];
    }

    // Optional: Create a method to get income for external access
    public float GetIncome(int playerNumber)
    {
        return playerIncomes.ContainsKey(playerNumber) ? playerIncomes[playerNumber] : 0f;
    }
}
