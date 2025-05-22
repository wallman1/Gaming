using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // or use TMPro if you're using TextMeshPro

public class LevelProgression : MonoBehaviour
{
    public float displayDuration = 3f;
    public int totalNotebooks = 5;
    private int notebooksRead = 0;
    private bool canExit = false;

    public PhotoCaptureSystem photoCaptureSystem;
    public GameObject notif;
    public Text scoreText; // Or use TMPro: public TextMeshProUGUI scoreText;

    void Start()
    {
        if (notif != null)
            notif.SetActive(false);

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);
    }

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
            StartCoroutine(NotifyAndAllowExit());
        }
    }

    private System.Collections.IEnumerator NotifyAndAllowExit()
    {
        notif.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        notif.SetActive(false);
        canExit = true;
    }

    public void InteractWithPlane()
    {
        if (canExit)
        {
            StartCoroutine(ShowScoreAndLoadNextLevel());
        }
        else
        {
            Debug.Log("You need to read all notebooks before using the plane.");
        }
    }

    private System.Collections.IEnumerator ShowScoreAndLoadNextLevel()
    {
        if (photoCaptureSystem != null)
        {
            float totalScore = photoCaptureSystem.GetTotalScore();
            int totalScorePercentage = Mathf.RoundToInt(totalScore * 100);
            Debug.Log("Level Finished! Total Photo Score: " + totalScorePercentage + "%");

            if (scoreText != null)
            {
                scoreText.text = $"Photo Score: {totalScorePercentage}%";
                scoreText.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(displayDuration);
        }
        else
        {
            Debug.LogWarning("PhotoCaptureSystem reference is missing!");
        }

        LoadNextLevel();
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
