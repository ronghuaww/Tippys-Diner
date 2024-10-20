using UnityEngine;
using UnityEngine.UI; // For Unity UI components
using TMPro; // TextMeshPro namespace
using UnityEngine.SceneManagement; // Add this for scene management
using System.Collections.Generic;
using System.Collections;

public class ResultsScreenManager : MonoBehaviour
{
    // Reference to UI elements
    public TMP_Text player1MoneyText; // TextMeshPro for displaying Player 1's money
    public TMP_Text player2MoneyText; // TextMeshPro for displaying Player 2's money
    public RectTransform player1Bar; // RectTransform for Player 1's income bar
    public RectTransform player2Bar; // RectTransform for Player 2's income bar
    public TMP_Text player1Label; // TextMeshPro for Player 1's label
    public TMP_Text player2Label; // TextMeshPro for Player 2's label
    public float heightMultiplier = 10f; // Multiplier to scale bar height for more dramatic effect
    public float animationDuration = 2f; // Duration for the height and number animation
    public string nextSceneName; // Name of the next scene to load

    private float targetPlayer1Income;
    private float targetPlayer2Income;
    private bool player1Confirmed = false;
    private bool player2Confirmed = false;

    void Awake()
    {
        IncomeManager.Instance.AddSalary(2);
    }
    private void Start()
    {
        // Set test incomes for debugging purposes
        // IncomeManager.Instance.SetTestIncome(1, 100f); // Example test value for Player 1
        // IncomeManager.Instance.SetTestIncome(2, 150f); // Example test value for Player 2

        // Access the singleton instance of IncomeManager and get the target incomes
        targetPlayer1Income = IncomeManager.Instance.GetIncome(1);
        targetPlayer2Income = IncomeManager.Instance.GetIncome(2);

        // Update the UI based on the final income values
        UpdateUI();
        StartCoroutine(AnimateIncomeBars()); // Start the animation
    }

    private void UpdateUI()
    {
        // Update the labels
        player1Label.text = "Player 1"; // Set initial label for Player 1
        player2Label.text = "Player 2"; // Set initial label for Player 2
    }

    private IEnumerator AnimateIncomeBars()
    {
        float elapsedTime = 0f;
        float initialHeight1 = player1Bar.sizeDelta.y;
        float initialHeight2 = player2Bar.sizeDelta.y;
        float targetHeight1 = targetPlayer1Income * heightMultiplier;
        float targetHeight2 = targetPlayer2Income * heightMultiplier;

        float currentPlayer1Income = 0f;
        float currentPlayer2Income = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            // Calculate the new height using Lerp
            float newHeight1 = Mathf.Lerp(initialHeight1, targetHeight1, elapsedTime / animationDuration);
            float newHeight2 = Mathf.Lerp(initialHeight2, targetHeight2, elapsedTime / animationDuration);

            // Update the bars
            SetBarHeight(player1Bar, newHeight1);
            SetBarHeight(player2Bar, newHeight2);

            // Update the displayed money amounts
            currentPlayer1Income = Mathf.Lerp(0, targetPlayer1Income, elapsedTime / animationDuration);
            currentPlayer2Income = Mathf.Lerp(0, targetPlayer2Income, elapsedTime / animationDuration);

            // Update the money text fields
            player1MoneyText.text = $"${currentPlayer1Income:F2}";
            player2MoneyText.text = $"${currentPlayer2Income:F2}";

            yield return null; // Wait for the next frame
        }

        // Ensure the bars and text reach their final values
        SetBarHeight(player1Bar, targetHeight1);
        SetBarHeight(player2Bar, targetHeight2);
        player1MoneyText.text = $"${targetPlayer1Income:F2}";
        player2MoneyText.text = $"${targetPlayer2Income:F2}";
    }

    private void SetBarHeight(RectTransform bar, float height)
    {
        // Ensure that the height does not go below zero
        height = Mathf.Max(height, 0f);
        Vector2 newSize = new Vector2(bar.sizeDelta.x, height);
        bar.sizeDelta = newSize; // Update the bar's height
    }

    private void Update()
    {
        // Check for confirmation key presses
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            TogglePlayer1Confirmation();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePlayer2Confirmation();
        }

        // Check if both players have confirmed
        if (player1Confirmed && player2Confirmed)
        {
            GameManager.Instance.LoadScene("Gameplay");
        }
    }

    private void TogglePlayer1Confirmation()
    {
        player1Confirmed = !player1Confirmed; // Toggle confirmation state
        player1Label.text = player1Confirmed ? "Confirmed" : "Player 1"; // Update label text
    }

    private void TogglePlayer2Confirmation()
    {
        player2Confirmed = !player2Confirmed; // Toggle confirmation state
        player2Label.text = player2Confirmed ? "Confirmed" : "Player 2"; // Update label text
    }

    private void LoadNextScene()
    {
        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
