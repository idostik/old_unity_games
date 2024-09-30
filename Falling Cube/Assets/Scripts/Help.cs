using UnityEngine.SceneManagement;
using UnityEngine;

public class Help : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
