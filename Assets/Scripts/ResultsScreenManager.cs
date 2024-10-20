using UnityEngine;
using UnityEngine.UI; // For Unity UI components
using TMPro; // TextMeshPro namespace
using UnityEngine.SceneManagement; // Add this for scene management
using System.Collections.Generic;
using System.Collections;

public class ResultsScreenManager : MonoBehaviour
{
    // Reference to UI elements
    public TMP_Text player1MoneyText;
    public TMP_Text player2MoneyText; 
    public RectTransform player1Bar;
    public RectTransform player2Bar; 
    public TMP_Text player1Label;
    public TMP_Text player2Label;
    public TMP_Text player1SubtractionText;
    public TMP_Text player2SubtractionText; 
    public TMP_Text brokeText;
    public TMP_Text surviveText;
    public float heightMultiplier = 10f; 
    public float animationDuration = 2f;
    public string nextSceneName; 

    private float targetPlayer1Income;
    private float targetPlayer2Income;
    private bool player1Confirmed = false;
    private bool player2Confirmed = false;
    private const float SUBTRACTION_AMOUNT = 32f;

    void Awake()
    {
        IncomeManager.Instance.AddSalary(2);
        brokeText.gameObject.SetActive(false);
        surviveText.gameObject.SetActive(false);
    }

    private void Start()
    {
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
        // Hide subtraction texts initially
        player1SubtractionText.gameObject.SetActive(false);
        player2SubtractionText.gameObject.SetActive(false);
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

        // Animate the initial income bars
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

         yield return new WaitForSeconds(3f);
        // Perform the subtraction
        StartCoroutine(ApplySubtraction());
    }

private IEnumerator ApplySubtraction()
{
    // Subtract the fixed amount from each player's income
    targetPlayer1Income -= SUBTRACTION_AMOUNT;
    targetPlayer2Income -= SUBTRACTION_AMOUNT;

    // Update the bar heights and text after subtraction
    float newHeight1 = Mathf.Max(targetPlayer1Income * heightMultiplier, 0);
    float newHeight2 = Mathf.Max(targetPlayer2Income * heightMultiplier, 0);

    SetBarHeight(player1Bar, newHeight1);
    SetBarHeight(player2Bar, newHeight2);
    
    // Update the subtraction texts
    player1SubtractionText.text = $"-${SUBTRACTION_AMOUNT:F2}";
    player2SubtractionText.text = $"-${SUBTRACTION_AMOUNT:F2}";

    // Make the subtraction texts visible
    player1SubtractionText.gameObject.SetActive(true);
    player2SubtractionText.gameObject.SetActive(true);

    // Wait for 5 seconds before starting to fade out
    yield return new WaitForSeconds(5f);

    // Start fading out the subtraction texts
    StartCoroutine(FadeOutSubtractionTexts(player1SubtractionText));
    StartCoroutine(FadeOutSubtractionTexts(player2SubtractionText));

    // Update the money text fields with the new values
    player1MoneyText.text = $"${targetPlayer1Income:F2}";
    player2MoneyText.text = $"${targetPlayer2Income:F2}";
}


    private IEnumerator FadeOutSubtractionTexts(TMP_Text subtractionText)
    {
        // Optional: Animate the fade-out effect
        Color originalColor = subtractionText.color;
        float elapsedTime = 0f;
        float fadeDuration = 1f; // Adjust the duration as needed

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            subtractionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        subtractionText.gameObject.SetActive(false); // Hide the text after fading out
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
        if(targetPlayer1Income <= 0){
            brokeText.text = $"Player 1 is BROKE";
            brokeText.gameObject.SetActive(true);
        }
        else if(targetPlayer2Income <= 0){
            brokeText.text = $"Player 2 is BROKE";
            brokeText.gameObject.SetActive(true);
        }
        else{
            surviveText.gameObject.SetActive(false);
        }
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
