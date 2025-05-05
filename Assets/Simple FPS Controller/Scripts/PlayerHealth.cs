using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void Kill()
    {
        Debug.Log("Player has died.");
        GameManager.Instance.GameOver();
        gameObject.SetActive(false); // Hide the player
    }
}
