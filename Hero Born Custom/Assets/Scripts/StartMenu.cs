using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Get the index of the current scene.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Load the next scene using the build index.
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
