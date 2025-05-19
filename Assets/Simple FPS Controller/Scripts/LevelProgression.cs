using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgression : MonoBehaviour
{
    public int totalNotebooks = 5;
    private int notebooksRead = 0;
    private bool canExit = false;
    public PhotoCaptureSystem photoCaptureSystem;

    public bool CanExitLevel()
    {
        return notebooksRead >= totalNotebooks;
    }

    public void NotebookRead()
    {
        notebooksRead++;
        Debug.Log($"Notebook read! {notebooksRead}/{totalNotebooks}");

        if (notebooksRead >= totalNotebooks)
        {
            Debug.Log("All notebooks read. You can now interact with the plane.");
            canExit = true;
        }
    }

    public void InteractWithPlane()
    {
        if (canExit)
        {
            if (photoCaptureSystem != null)
            {
                float totalScore = photoCaptureSystem.GetTotalScore();
                int totalScorePercentage = Mathf.RoundToInt(totalScore * 100);
                Debug.Log("Level Finished! Total Photo Score: " + totalScorePercentage + "%");
            }
            else
            {
                Debug.LogWarning("PhotoCaptureSystem reference is missing!");
            }
            Debug.Log("Interacted with the plane. Loading next level...");
            LoadNextLevel();
        }
        else
        {
            Debug.Log("You need to read all notebooks before using the plane.");
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
