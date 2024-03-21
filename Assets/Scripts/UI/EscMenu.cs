using UnityEngine.SceneManagement;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    
    public void GoToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings(){
        
    }

    public void ReturnToGame(){
        Controller_Main.instance.UnPauseLevel();
    }
}
