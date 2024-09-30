using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour {
    

   

	public void Restart ()  //načte tu samou scénu
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
		
	
}
