using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject gameOverUI;
    public GameObject youWinUI;


    bool gameHasEnded = false;
    bool youWin = false;

    private void Awake()        //při zapnutí hry vypne výherní i konečnou obrazovku
    {
        gameOverUI.SetActive(false);
        youWinUI.SetActive(false);
    }

    public void GameOver ()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;    //zabráním tím vícenásobnému zapnutí konečné obrazovky
            gameOverUI.SetActive(true);
            youWin = true; //zabráním tím aby se zobrazila výherní obrazovka když hráč zemře a poté spadne do cíle
        }
            
    }

    public void YouWin ()
    {
        if (youWin == false)
        {
            youWin = true;  //zabráním tím vícenásobnému zapnutí výherní obrazovky
            youWinUI.SetActive(true);
        }
    }

  
}
