using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class PhotoCaptureSystem : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera playerCamera;
    public float detectionRange = 100f;
    public string[] detectableTags;
    public RenderTexture renderTexture;

    [Header("Photo Settings")]
    public string saveFolderName = "Photos";
    public RawImage flashUI;
    public float flashDuration = 0.2f;
    public GameObject photoPrefab;
    public Transform galleryPanel;
    public GameObject gallery;

    [Header("Gallery Pagination")]
    public int photosPerPage = 6;

    private int currentPage = 0;
    private List<GameObject> galleryItems = new List<GameObject>();

    private int photoCount = 0;
    private List<Texture2D> photoGallery = new List<Texture2D>();

    void Start()
    {
        gallery.SetActive(false);
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(TakePhotoRoutine());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            gallery.SetActive(!gallery.activeSelf);
            if (gallery.activeSelf)
            {
                RefreshGalleryView(); // Show correct page when opened
            }
    }

    // Only respond to page keys if gallery is open
    if (gallery.activeSelf)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            PreviousPage();
        }
    }
    }

    IEnumerator TakePhotoRoutine()
    {
        string detectedTag = DetectObject(out float score);
        yield return StartCoroutine(FlashScreen());

        Texture2D photo = CapturePhoto();
        SavePhoto(photo, detectedTag, score);
        AddPhotoToGallery(photo, score);
    }

    string DetectObject(out float score)
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        score = 0f;

        if (Physics.Raycast(ray, out hit, detectionRange))
        {
            foreach (string tag in detectableTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    // Score based on how centered the object is in view
                    Vector3 viewPos = playerCamera.WorldToViewportPoint(hit.point);
                    float distanceFromCenter = Vector2.Distance(new Vector2(0.5f, 0.5f), new Vector2(viewPos.x, viewPos.y));
                    score = (1f - (distanceFromCenter*100000)); // 1 = perfect center, 0 = far off
                    return tag;
                }
            }
            return "Unknown";
        }
        return "None";
    }

    IEnumerator FlashScreen()
    {
        if (flashUI != null)
        {
            flashUI.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(flashDuration);
            flashUI.color = new Color(1, 1, 1, 0);
        }
    }

    Texture2D CapturePhoto()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        playerCamera.targetTexture = renderTexture;
        playerCamera.Render();

        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        playerCamera.targetTexture = null;
        RenderTexture.active = currentRT;

        return image;
    }

    void SavePhoto(Texture2D image, string detectedObject, float score)
    {
        byte[] bytes = image.EncodeToPNG();
        string folderPath = Path.Combine(Application.dataPath, saveFolderName);
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string fileName = $"Photo_{photoCount}_{detectedObject}_Score{Mathf.RoundToInt(score * 100)}.png";
        File.WriteAllBytes(Path.Combine(folderPath, fileName), bytes);
        Debug.Log($" Saved {fileName} with score: {score * 100}%");

        photoCount++;
    }

    public void NextPage()
    {
        if ((currentPage + 1) * photosPerPage < galleryItems.Count)
        {
            currentPage++;
            RefreshGalleryView();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshGalleryView();
        }
    }

    void RefreshGalleryView()
    {
        for (int i = 0; i < galleryItems.Count; i++)
        {
            bool isVisible = i >= currentPage * photosPerPage && i < (currentPage + 1) * photosPerPage;
            galleryItems[i].SetActive(isVisible);
        }
    }

    void AddPhotoToGallery(Texture2D image, float score)
    {
    if (photoPrefab != null && galleryPanel != null)
    {
        GameObject newPhoto = Instantiate(photoPrefab);
        newPhoto.transform.SetParent(galleryPanel, false);
        RawImage img = newPhoto.GetComponentInChildren<RawImage>();
        if (img != null)
            img.texture = image;
        else
            Debug.LogError("No RawImage found in photoPrefab!");

        Text scoreText = newPhoto.GetComponentInChildren<Text>();
        if (scoreText != null)
        {
            scoreText.text = $"Score: {Mathf.RoundToInt(score * 100)}%";
        }

        galleryItems.Add(newPhoto);
        RefreshGalleryView();
    }

    }
}
