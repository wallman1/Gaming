using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;

    public GameObject notebookPanel;
    public TextMeshProUGUI notebookContent;
    public KeyCode closeKey = KeyCode.Escape;

    private bool isOpen = false;

    void Awake()
    {
        Instance = this;
        notebookPanel.SetActive(false);
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(closeKey))
        {
            HideNotebook();
        }
    }

    public void ShowNotebook(string text)
    {
        notebookContent.text = text;
        notebookPanel.SetActive(true);
        isOpen = true;
        Time.timeScale = 0f; // optional: pause game while reading
    }

    public void HideNotebook()
    {
        notebookPanel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;
    }
}
