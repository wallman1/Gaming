using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Notebook : MonoBehaviour, IInteractable
{
    [TextArea(5, 10)]
    public string Pg1;
    public string Pg2;
    public string Pg3;

    private bool hasBeenRead = false;
    private string[] pages;

    public void Interact()
    {
        if (NotebookUI.Instance == null) return;

        pages = new string[] { Pg1, Pg2, Pg3 };
        NotebookUI.Instance.ShowNotebook(pages, this); // Pass itself to UI
    }

    public void MarkAsRead()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;
            FindObjectOfType<LevelProgression>()?.NotebookRead();
            Debug.Log($"Notebook '{gameObject.name}' marked as read.");
        }
    }
}

