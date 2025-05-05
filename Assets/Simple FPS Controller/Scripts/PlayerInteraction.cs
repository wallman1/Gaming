using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public KeyCode interactKey = KeyCode.Q;

    public TextMeshProUGUI interactionPrompt; // Reference to the UI prompt

    private Camera cam;
    private IInteractable currentInteractable;

    void Start()
    {
        cam = Camera.main;
        interactionPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactionPrompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                {
                    currentInteractable.Interact();
                    interactionPrompt.gameObject.SetActive(false);
                }

                return;
            }
        }

        currentInteractable = null;
        interactionPrompt.gameObject.SetActive(false);
    }
}
