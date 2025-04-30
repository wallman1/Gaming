using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Notebook : MonoBehaviour, IInteractable
{
    [TextArea(5, 10)]
    public string notebookText;

    public void Interact()
    {
        NotebookUI.Instance.ShowNotebook(notebookText);
    }
}
