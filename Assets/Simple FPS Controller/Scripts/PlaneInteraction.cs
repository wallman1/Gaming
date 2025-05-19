using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        LevelProgression progressionManager = FindObjectOfType<LevelProgression>();

        if (progressionManager == null)
        {
            Debug.LogError("LevelProgressionManager not found in the scene!");
            return;
        }

        if (progressionManager.CanExitLevel())
        {
            Debug.Log("All notebooks read. Loading next level...");
            LoadNextScene();
        }
        else
        {
            Debug.Log("You must read all notebooks before using the plane.");
        }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("This is the last scene. No more scenes to load.");
        }
    }
}
