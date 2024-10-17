using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

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

    // Start is called before the first frame update
    void Start()
    {
        // You can initialize any starting logic here
    }

    // Update is called once per frame
    void Update()
    {
        // You can handle per-frame logic here
    }

    // Method to handle scene transitions
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Optional: Add loading logic (like showing a loading screen)

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene loading is complete
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
