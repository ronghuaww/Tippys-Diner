using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

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

    // Method to load a scene by name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene loading is complete
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
