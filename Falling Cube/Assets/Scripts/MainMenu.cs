using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void PlayGame ()     //načte scénu kterí je v BuildSetting o 2 dál -> Hra
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Help ()     //načte scénu kterí je v BuildSetting o 1 dál -> Help
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }
}
