using System.Collections.Generic;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    private Dictionary<int, float> playerIncomes = new Dictionary<int, float>();

    // Make sure the inspector can show the incomes for player 1 and player 2
    [SerializeField] private float player1Income;
    [SerializeField] private float player2Income;

    private void Start()
    {
        // Initialize incomes for two players
        playerIncomes[1] = 0f;
        playerIncomes[2] = 0f;
    }

    public void AddIncome(int playerNumber, float amount)
    {
        if (playerIncomes.ContainsKey(playerNumber))
        {
            playerIncomes[playerNumber] += amount;
            UpdatePlayerIncomeProperties(); // Update the serialized fields
        }
        else
        {
            Debug.LogWarning("Invalid player number for income!");
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
