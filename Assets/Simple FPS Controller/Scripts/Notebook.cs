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

    private string[] nah;

    public void Interact()
    {
        // Initialize the page array just before showing the notebook
        nah = new string[] { Pg1, Pg2, Pg3 };
        NotebookUI.Instance.ShowNotebook(nah);
    }
}
