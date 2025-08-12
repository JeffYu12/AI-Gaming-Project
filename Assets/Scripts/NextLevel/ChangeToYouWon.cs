using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToYouWonOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure player GameObject is tagged "Player"
        {
            SceneManager.LoadScene("YouWon");
        }
    }
}
