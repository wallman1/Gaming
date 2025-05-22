using UnityEngine;

public class Popupcontroller : MonoBehaviour
{
    public GameObject popupPanel;         // Assign this in the Inspector
    public float displayDuration = 3f;    // Time the popup stays on screen

    private void Start()
    {
        popupPanel.SetActive(false);     // Ensure popup is hidden at start
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
            StartCoroutine(ShowPopupCoroutine());
    }

    private System.Collections.IEnumerator ShowPopupCoroutine()
    {
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        popupPanel.SetActive(false);
    }
}

