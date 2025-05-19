using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;

    public GameObject notebookPanel;
    public TextMeshProUGUI notebookContent;
    public KeyCode closeKey = KeyCode.Escape;
    public RawImage Background;

    private bool isOpen = false;
    private int currentPage = 0;
    private string[] notebookPages;
    private Notebook currentNotebook;

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

    if (isOpen)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (currentPage < notebookPages.Length - 1)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentPage > 0)
            {
                currentPage--;
                DisplayCurrentPage();
            }
        }
    }
    }

    private void DisplayCurrentPage()
    {
        if (notebookPages != null && currentPage >= 0 && currentPage < notebookPages.Length)
        {
            notebookContent.text = notebookPages[currentPage];
        }
    }

        public void ShowNotebook(string[] pages, Notebook sourceNotebook)
    {
        if (pages == null || pages.Length == 0) return;

        notebookPages = pages;
        currentNotebook = sourceNotebook; // NEW: store reference
        currentPage = 0;
        notebookPanel.SetActive(true);
        isOpen = true;
        Time.timeScale = 0f;
        DisplayCurrentPage();
    }



    public void HideNotebook()
    {
        notebookPanel.SetActive(false);
        isOpen = false;
        Time.timeScale = 1f;

        if (currentNotebook != null)
        {
            currentNotebook.MarkAsRead();
            currentNotebook = null;
        }
    }

}
