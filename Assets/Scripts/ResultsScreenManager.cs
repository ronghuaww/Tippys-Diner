using UnityEngine;
using UnityEngine.UI; // Add this line for Unity UI components
using TMPro; // TextMeshPro namespace
using System.Collections;

public class ResultsScreenManager : MonoBehaviour
{
    // Reference to UI elements
    public TMP_Text player1MoneyText; // TextMeshPro for displaying Player 1's money
    public TMP_Text player2MoneyText; // TextMeshPro for displaying Player 2's money
    public Image player1Bar; // Image for Player 1's income bar
    public Image player2Bar; // Image for Player 2's income bar
    public float animationDuration = 2f; // Duration of the income animation

    private float targetPlayer1Income;
    private float targetPlayer2Income;
    private float currentPlayer1Income;
    private float currentPlayer2Income;

    private void Start()
    {
        // Access the singleton instance of IncomeManager
        targetPlayer1Income = IncomeManager.Instance.GetIncome(1);
        targetPlayer2Income = IncomeManager.Instance.GetIncome(2);
        ResetIncomeBars(); // Reset the income bars to zero before starting the animation
        StartCoroutine(AnimateIncomeBars());
    }

    private void ResetIncomeBars()
    {
        // Reset current incomes and UI elements
        currentPlayer1Income = 0f;
        currentPlayer2Income = 0f;

        player1Bar.fillAmount = 0f;
        player2Bar.fillAmount = 0f;
        player1MoneyText.text = "$0.00"; // Initialize the money text to $0.00
        player2MoneyText.text = "$0.00"; // Initialize the money text to $0.00
    }

    private IEnumerator AnimateIncomeBars()
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            currentPlayer1Income = Mathf.Lerp(0, targetPlayer1Income, elapsedTime / animationDuration);
            currentPlayer2Income = Mathf.Lerp(0, targetPlayer2Income, elapsedTime / animationDuration);

            UpdateUI();

            yield return null;
        }

        // Ensure the bars reach their final value
        currentPlayer1Income = targetPlayer1Income;
        currentPlayer2Income = targetPlayer2Income;
        UpdateUI();
    }

    private void UpdateUI()
    {
        player1Bar.fillAmount = currentPlayer1Income / targetPlayer1Income;
        player2Bar.fillAmount = currentPlayer2Income / targetPlayer2Income;

        // Update the separate money text fields
        player1MoneyText.text = $"${currentPlayer1Income:F2}";
        player2MoneyText.text = $"${currentPlayer2Income:F2}";
    }
}
