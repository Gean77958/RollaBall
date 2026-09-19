using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RestartGame()
    {
        // Recarga la escena actual desde el principio
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
